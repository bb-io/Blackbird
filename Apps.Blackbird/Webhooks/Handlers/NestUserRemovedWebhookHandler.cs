using Apps.Blackbird.Webhooks.Models;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Blackbird.Webhooks.Handlers;

public class NestUserRemovedWebhookHandler : BlackbirdWebhookHandler
{
    protected override string EventType => "nest_user_removed";

    public NestUserRemovedWebhookHandler(InvocationContext invocationContext, [WebhookParameter(true)] NestRequiredWebhookInput nestRequest) :
        base(invocationContext, nestRequest.NestId)
    {
    }
}