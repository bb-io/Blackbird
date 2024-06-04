using Apps.Blackbird.Webhooks.Models;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Blackbird.Webhooks.Handlers;

public class NestCreatedWebhookHandler : BlackbirdWebhookHandler
{
    protected override string EventType => "nest_created";

    public NestCreatedWebhookHandler(InvocationContext invocationContext, [WebhookParameter] NestWebhookInput nestRequest) :
        base(invocationContext, nestRequest.NestId)
    {
    }
}