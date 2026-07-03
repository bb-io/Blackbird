using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Blackbird.Webhooks.Handlers;

public class NotificationCreatedWebhookHandler : BlackbirdWebhookHandler
{
    protected override string EventType => "notification_created";

    public NotificationCreatedWebhookHandler(InvocationContext invocationContext) :
        base(invocationContext, null)
    {
    }
}