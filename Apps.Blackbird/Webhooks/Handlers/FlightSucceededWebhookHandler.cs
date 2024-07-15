using Apps.Blackbird.Webhooks.Models;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Blackbird.Webhooks.Handlers;

public class FlightSucceededWebhookHandler : BlackbirdWebhookHandler
{
    protected override string EventType => "flight_succeeded";

    public FlightSucceededWebhookHandler(InvocationContext invocationContext,
        [WebhookParameter(true)] NestWebhookInput nestRequest) :
        base(invocationContext, nestRequest.NestId)
    {
    }
}