using Apps.Blackbird.Webhooks.Models;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Blackbird.Webhooks.Handlers;

public class BirdPublishedWebhookHandler : BlackbirdWebhookHandler
{
    protected override string EventType => "bird_published";

    public BirdPublishedWebhookHandler(InvocationContext invocationContext,
        [WebhookParameter] NestWebhookInput nestRequest) :
        base(invocationContext, nestRequest.NestId)
    {
    }
}