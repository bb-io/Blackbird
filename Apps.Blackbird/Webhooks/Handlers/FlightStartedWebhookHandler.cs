using Apps.Blackbird.Webhooks.Models;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Blackbird.Webhooks.Handlers;

public class FlightStartedWebhookHandler : BlackbirdWebhookHandler
{
    protected override string EventType => "flight_started";

    public FlightStartedWebhookHandler(InvocationContext invocationContext,
        [WebhookParameter] NestWebhookInput nestRequest) :
        base(invocationContext, nestRequest.NestId)
    {
    }
}