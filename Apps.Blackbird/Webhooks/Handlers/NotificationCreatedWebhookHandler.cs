using Apps.Blackbird.Webhooks.Models;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Blackbird.Webhooks.Handlers;

public class NotificationCreatedWebhookHandler : BlackbirdWebhookHandler
{
    protected override string EventType => "notification_created";

    public NotificationCreatedWebhookHandler(InvocationContext invocationContext, [WebhookParameter(true)] NestRequiredWebhookInput nestRequest) :
        base(invocationContext, nestRequest.NestId)
    {
    }
}