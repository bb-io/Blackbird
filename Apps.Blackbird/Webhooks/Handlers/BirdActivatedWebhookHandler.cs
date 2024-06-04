using Apps.Blackbird.Webhooks.Models;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Blackbird.Webhooks.Handlers;

public class BirdActivatedWebhookHandler : BlackbirdWebhookHandler
{
    protected override string EventType => "bird_activated";

    public BirdActivatedWebhookHandler(InvocationContext invocationContext,
        [WebhookParameter] NestWebhookInput nestRequest) :
        base(invocationContext, nestRequest.NestId)
    {
    }
}