using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Blackbird.Webhooks.Handlers;

public class NestDeletedWebhookHandler : BlackbirdWebhookHandler
{
    protected override string EventType => "nest_deleted";

    public NestDeletedWebhookHandler(InvocationContext invocationContext) :
        base(invocationContext, null)
    {
    }
}