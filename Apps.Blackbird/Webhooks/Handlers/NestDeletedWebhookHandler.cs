using Apps.Blackbird.Webhooks.Models;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Blackbird.Webhooks.Handlers;

public class NestDeletedWebhookHandler : BlackbirdWebhookHandler
{
    protected override string EventType => "nest_deleted";

    public NestDeletedWebhookHandler(InvocationContext invocationContext,
        [WebhookParameter] NestWebhookInput nestRequest) :
        base(invocationContext, nestRequest.NestId)
    {
    }
}