using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Blackbird.Webhooks.Handlers;

public class NestCreatedWebhookHandler : BlackbirdWebhookHandler
{
    protected override string EventType => "nest_created";

    public NestCreatedWebhookHandler(InvocationContext invocationContext) :
        base(invocationContext, null)
    {
    }
}