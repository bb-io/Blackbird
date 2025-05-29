using System.Net;
using Apps.Blackbird.Api;
using Apps.Blackbird.Invocables;
using Apps.Blackbird.Models.Entities;
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

    [Webhook("On nest created", typeof(NestCreatedWebhookHandler), Description = "On a new nest created")]
    public Task<WebhookResponse<NestEntity>> OnNestCreated(WebhookRequest request)
        => ProcessWebhook<NestEntity>(request);

    [Webhook("On nest deleted", typeof(NestDeletedWebhookHandler), Description = "On a specific nest deleted")]
    public Task<WebhookResponse<NestEntity>> OnNestDeleted(WebhookRequest request)
        => ProcessWebhook<NestEntity>(request);

    [Webhook("On user added to nest", typeof(NestUserAddedWebhookHandler),
        Description = "On a new user added to the nest")]
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

    [Webhook("On user removed from nest", typeof(NestUserRemovedWebhookHandler),
        Description = "On a specific user removed from the nest")]
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

    [Webhook("On bird published", typeof(BirdPublishedWebhookHandler), Description = "On a specific bird published")]
    public Task<WebhookResponse<BirdWrapperResponse>> OnBirdPublished(WebhookRequest request) => ProcessBirdWebhook(request);

    [Webhook("On bird suspended", typeof(BirdSuspendedWebhookHandler), Description = "On a specific bird suspended")]
    public Task<WebhookResponse<BirdWrapperResponse>> OnBirdSuspended(WebhookRequest request) => ProcessBirdWebhook(request);

    [Webhook("On bird activated", typeof(BirdActivatedWebhookHandler), Description = "On a specific bird activated")]
    public Task<WebhookResponse<BirdWrapperResponse>> OnBirdActivated(WebhookRequest request) => ProcessBirdWebhook(request);

    [Webhook("On flight started", typeof(FlightStartedWebhookHandler), Description = "On a new flight started")]
    public Task<WebhookResponse<FlightWrapperResponse>> OnFlightStarted(WebhookRequest request) => ProcessFlightWebhook(request);

    [Webhook("On flight succeeded", typeof(FlightSucceededWebhookHandler),Description = "On a specific flight succeeded")]
    public Task<WebhookResponse<FlightWrapperResponse>> OnFlightSucceeded(WebhookRequest request) => ProcessFlightWebhook(request);

    [Webhook("On flight failed", typeof(FlightFailedWebhookHandler), Description = "On a specific flight failed")]
    public Task<WebhookResponse<FlightWrapperResponse>> OnFlightFailed(WebhookRequest request,
        [WebhookParameter(true)] BirdWebhookRequest filter) => ProcessFlightWebhook(request, filter.BirdId);

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
        string? birdIdFilter=null)
    {
        var result = await ProcessWebhook<FlightEntity>(request);
        var flight = result.Result;

        if (!string.IsNullOrWhiteSpace(birdIdFilter)
        && !string.Equals(birdIdFilter, flight?.BirdId, StringComparison.OrdinalIgnoreCase))
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