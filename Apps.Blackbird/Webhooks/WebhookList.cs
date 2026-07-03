using System.Net;
using Apps.Blackbird.Api;
using Apps.Blackbird.Invocables;
using Apps.Blackbird.Models.Entities;
using Apps.Blackbird.Models.Events;
using Apps.Blackbird.Models.Request.Birds;
using Apps.Blackbird.Webhooks.Handlers;
using Apps.Blackbird.Webhooks.Models;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Blackbird.Webhooks;

[WebhookList]
public class WebhookList : BlackbirdAppInvocable
{
    public WebhookList(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Webhook("On Nest created", typeof(NestCreatedWebhookHandler), Description = "On a Nest created")]
    public Task<WebhookResponse<NestEntity>> OnNestCreated(WebhookRequest request)
        => ProcessWebhook<NestEntity>(request);

    [Webhook("On Nest deleted", typeof(NestDeletedWebhookHandler), Description = "On a Nest deleted")]
    public Task<WebhookResponse<NestEntity>> OnNestDeleted(WebhookRequest request)
        => ProcessWebhook<NestEntity>(request);

    [Webhook("On user added to Nest", typeof(NestUserAddedWebhookHandler),
        Description = "On a user added to a Nest")]
    public async Task<WebhookResponse<NestEntity>> OnUserAddedToNest(WebhookRequest request)
    {
        var payload = request.Body.ToString();
        ArgumentException.ThrowIfNullOrWhiteSpace(payload);

        var data = JsonConvert.DeserializeObject<BlackbirdWebhookPayload<UserEntity>>(payload)!;

        var nestRequest = new BlackbirdAppRequest($"nests/{data.NestId}", Method.Get, Creds);
        var result = await Client.ExecuteWithErrorHandling<NestEntity>(nestRequest);

        return new()
        {
            Result = result,
            HttpResponseMessage = new(HttpStatusCode.OK)
        };
    }

    [Webhook("On user removed from Nest", typeof(NestUserRemovedWebhookHandler),
        Description = "On a user removed from a Nest")]
    public Task<WebhookResponse<UserWebhookResponse>> OnUserRemovedFromNest(WebhookRequest request)
    {
        var payload = request.Body.ToString();
        ArgumentException.ThrowIfNullOrWhiteSpace(payload);

        var data = JsonConvert.DeserializeObject<BlackbirdWebhookPayload<UserWebhookResponse>>(payload)!;
        data.Entity.NestId = data.NestId;

        return Task.FromResult(new WebhookResponse<UserWebhookResponse>()
        {
            Result = data.Entity,
            HttpResponseMessage = new(HttpStatusCode.OK)
        });
    }

    [Webhook("On Bird published", typeof(BirdPublishedWebhookHandler), Description = "On a Bird published")]
    public Task<WebhookResponse<BirdWrapperResponse>> OnBirdPublished(WebhookRequest request) => ProcessBirdWebhook(request);

    [Webhook("On Bird suspended", typeof(BirdSuspendedWebhookHandler), Description = "On a Bird suspended")]
    public Task<WebhookResponse<BirdWrapperResponse>> OnBirdSuspended(WebhookRequest request) => ProcessBirdWebhook(request);

    [Webhook("On Bird activated", typeof(BirdActivatedWebhookHandler), Description = "On a Bird activated")]
    public Task<WebhookResponse<BirdWrapperResponse>> OnBirdActivated(WebhookRequest request) => ProcessBirdWebhook(request);

    [Webhook("On Flight started", typeof(FlightStartedWebhookHandler), Description = "On a Flight started")]
    public async Task<WebhookResponse<FlightWrapperResponse>> OnFlightStarted(WebhookRequest request,
        [WebhookParameter(true)] BirdWebhookRequest filter)

    {
        if (await ShouldPreflightAsync(request, filter))
        {
            return new WebhookResponse<FlightWrapperResponse>
            {
                HttpResponseMessage = new(HttpStatusCode.OK),
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        return await ProcessFlightWebhook(request, filter.BirdIds?.ToArray() ?? []);
    }

    [Webhook("On Flight succeeded", typeof(FlightSucceededWebhookHandler), Description = "On a Flight succeeded")]
    public async Task<WebhookResponse<FlightWrapperResponse>> OnFlightSucceeded(WebhookRequest request,
        [WebhookParameter(true)] BirdWebhookRequest filter)

    {
        if (await ShouldPreflightAsync(request, filter))
        {
            return new WebhookResponse<FlightWrapperResponse>
            {
                HttpResponseMessage = new(HttpStatusCode.OK),
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        return await ProcessFlightWebhook(request, filter.BirdIds?.ToArray() ?? []);
    }

    [Webhook("On Flight failed", typeof(FlightFailedWebhookHandler), Description = "On a Flight failed")]
    public async Task<WebhookResponse<FlightWrapperResponse>> OnFlightFailed(WebhookRequest request,
        [WebhookParameter(true)] BirdWebhookRequest filter)
    {
        if (await ShouldPreflightAsync(request, filter))
        {
            return new WebhookResponse<FlightWrapperResponse>
            {
                HttpResponseMessage = new(HttpStatusCode.OK),
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        return await ProcessFlightWebhook(request, filter.BirdIds?.ToArray() ?? []);
    }

    [Webhook("On Notification received", typeof(NotificationCreatedWebhookHandler), Description = "On a notification received")]
    public async Task<WebhookResponse<NotificationPayload>> OnNotificationReceived(WebhookRequest request, [WebhookParameter] NotificationTypeRequest notificationType)
    {
        var payload = request.Body.ToString();
        var wrapper = JsonConvert.DeserializeObject<BlackbirdWebhookPayload<string>>(payload)!;
        var data = JsonConvert.DeserializeObject<NotificationPayload>(wrapper.Entity)!;

        if (notificationType.EventType is not null && notificationType.EventType != "all")
        {
            if (notificationType.EventType != data.EventType)
            {
                return new WebhookResponse<NotificationPayload>
                {
                    HttpResponseMessage = new(HttpStatusCode.OK),
                    ReceivedWebhookRequestType = WebhookRequestType.Preflight
                };
            }
        }

        if (data.WorkflowId is not null)
        {
            var bird = await LoadBirdEntityById(data.WorkflowId.ToString(), data.WorkspaceId.ToString());
            data.BirdName = bird?.Name;
        }

        return new WebhookResponse<NotificationPayload>
        {
            HttpResponseMessage = new(HttpStatusCode.OK),
            Result = data
        };
    }

    private async Task<bool> ShouldPreflightAsync(WebhookRequest request, BirdWebhookRequest filter)
    {
        try
        {
            var payload = request.Body?.ToString();
            if (string.IsNullOrWhiteSpace(payload))
                return true;

            var data = JsonConvert.DeserializeObject<BlackbirdWebhookPayload<FlightEntity>>(payload);
            var flight = data?.Entity;
            if (flight is null)
                return true;

            if (flight.BirdId == InvocationContext.Bird?.Id.ToString())
                return true;

            if (filter.BirdIds != null && filter.BirdIds.Any() && !filter.BirdIds.Contains(flight.BirdId))
                return true;

            if (!string.IsNullOrWhiteSpace(filter?.NestId) &&
                !string.Equals(filter.NestId, flight.NestId, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrWhiteSpace(filter?.BirdStatus))
            {
                var bird = await LoadBirdEntityById(flight.BirdId, flight.NestId);
                if (!string.Equals(bird?.Status, filter.BirdStatus, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
        catch
        {
            return true;
        }
    }

    private Task<WebhookResponse<T>> ProcessWebhook<T>(WebhookRequest request) where T : class
    {
        var payload = request.Body.ToString();
        ArgumentException.ThrowIfNullOrWhiteSpace(payload);

        var data = JsonConvert.DeserializeObject<BlackbirdWebhookPayload<T>>(payload)!;
        return Task.FromResult(new WebhookResponse<T>()
        {
            Result = data.Entity,
            HttpResponseMessage = new(HttpStatusCode.OK)
        });
    }

    private async Task<WebhookResponse<BirdWrapperResponse>> ProcessBirdWebhook(WebhookRequest request)
    {
        var result = await ProcessWebhook<BirdEntity>(request);
        var bird = result.Result;

        if (bird?.Id == InvocationContext.Bird?.Id.ToString())
        {
            return new WebhookResponse<BirdWrapperResponse>
            {
                HttpResponseMessage = new(HttpStatusCode.OK),
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        var nest = await LoadNestEntityById(bird.NestId);

        var wrapper = new BirdWrapperResponse
        {
            Id = bird.Id,
            Nest = nest,
            Name = bird.Name,
            Status = bird.Status,
            TriggerType = bird.TriggerType,
            IsPublished = bird.IsPublished
        };

        return new WebhookResponse<BirdWrapperResponse>
        {
            HttpResponseMessage = new(HttpStatusCode.OK),
            Result = wrapper
        };
    }

    private async Task<WebhookResponse<FlightWrapperResponse>> ProcessFlightWebhook(WebhookRequest request,
        params string[] birdIdFilter)
    {
        var result = await ProcessWebhook<FlightEntity>(request);
        var flight = result.Result;

        if (birdIdFilter.Length != 0 && !birdIdFilter.Contains(flight?.BirdId))
        {
            return new WebhookResponse<FlightWrapperResponse>
            {
                HttpResponseMessage = new(HttpStatusCode.OK),
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        if (flight?.BirdId == InvocationContext.Bird?.Id.ToString())
            return new()
            {
                HttpResponseMessage = new(HttpStatusCode.OK),
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };

        var bird = await LoadBirdEntityById(flight.BirdId, flight.NestId);
        var nest = await LoadNestEntityById(flight.NestId);

        var wrapper = new FlightWrapperResponse
        {
            Id = flight.Id,
            Bird = bird,
            Nest = nest,
            Status = flight.Status,
            ErrorMessages = flight.ErrorMessages,
            StartDate = flight.StartDate,
            Duration = flight.Duration
        };

        return new()
        {
            HttpResponseMessage = result.HttpResponseMessage,
            Result = wrapper
        };
    }

    private async Task<BirdEntity?> LoadBirdEntityById(string? birdId, string? nestId)
    {
        if (string.IsNullOrWhiteSpace(birdId))
            return null;

        var birdRequest = new BlackbirdAppRequest($"nests/{nestId}/birds/{birdId}", Method.Get, Creds);
        return await Client.ExecuteWithErrorHandling<BirdEntity>(birdRequest);
    }

    private async Task<NestEntity?> LoadNestEntityById(string? nestId)
    {
        if (string.IsNullOrWhiteSpace(nestId))
            return null;

        var nestRequest = new BlackbirdAppRequest($"nests/{nestId}", Method.Get, Creds);
        return await Client.ExecuteWithErrorHandling<NestEntity>(nestRequest);
    }

}
