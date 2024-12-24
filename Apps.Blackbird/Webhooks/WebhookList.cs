using System.Net;
using Apps.Blackbird.Api;
using Apps.Blackbird.Invocables;
using Apps.Blackbird.Models.Entities;
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
    public Task<WebhookResponse<BirdEntity>> OnBirdPublished(WebhookRequest request) => ProcessBirdWebhook(request);

    [Webhook("On bird suspended", typeof(BirdSuspendedWebhookHandler), Description = "On a specific bird suspended")]
    public Task<WebhookResponse<BirdEntity>> OnBirdSuspended(WebhookRequest request) => ProcessBirdWebhook(request);

    [Webhook("On bird activated", typeof(BirdActivatedWebhookHandler), Description = "On a specific bird activated")]
    public Task<WebhookResponse<BirdEntity>> OnBirdActivated(WebhookRequest request) => ProcessBirdWebhook(request);
    //
    [Webhook("On flight started", typeof(FlightStartedWebhookHandler), Description = "On a new flight started")]
    public Task<WebhookResponse<FlightEntity>> OnFlightStarted(WebhookRequest request) => ProcessFlightWebhook(request);

    [Webhook("On flight succeeded", typeof(FlightSucceededWebhookHandler),Description = "On a specific flight succeeded")]
    public Task<WebhookResponse<FlightEntity>> OnFlightSucceeded(WebhookRequest request) => ProcessFlightWebhook(request);

    [Webhook("On flight failed", typeof(FlightFailedWebhookHandler), Description = "On a specific flight failed")]
    public Task<WebhookResponse<FlightEntity>> OnFlightFailed(WebhookRequest request) => ProcessFlightWebhook(request);

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

    private async Task<WebhookResponse<BirdEntity>> ProcessBirdWebhook(WebhookRequest request)
    {
        var result = await ProcessWebhook<BirdEntity>(request);

        if (result.Result?.Id == InvocationContext.Bird?.Id.ToString())
            return new()
            {
                HttpResponseMessage = new(HttpStatusCode.OK),
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };

        return result;
    }

    private async Task<WebhookResponse<FlightEntity>> ProcessFlightWebhook(WebhookRequest request)
    {
        var result = await ProcessWebhook<FlightEntity>(request);

        if (result.Result?.BirdId == InvocationContext.Bird?.Id.ToString())
            return new()
            {
                HttpResponseMessage = new(HttpStatusCode.OK),
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };

        return result;
    }
}