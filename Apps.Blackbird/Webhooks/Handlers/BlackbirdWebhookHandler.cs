using Apps.Blackbird.Api;
using Apps.Blackbird.Invocables;
using Apps.Blackbird.Models.Entities;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Blackbird.Webhooks.Handlers;

public abstract class BlackbirdWebhookHandler : BlackbirdAppInvocable, IWebhookEventHandler
{
    private readonly string? _nestId;
    protected abstract string EventType { get; }
    
    public BlackbirdWebhookHandler(InvocationContext invocationContext, string? nestId) : base(invocationContext)
    {
        _nestId = nestId;
    }
    
    public Task SubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider, Dictionary<string, string> values)
    {
        var request = new BlackbirdAppRequest("/webhooks", Method.Post, authenticationCredentialsProvider)
            .WithJsonBody(new
            {
                eventType = EventType,
                callbackUrl = values["payloadUrl"],
                nestId = _nestId
            });

        return Client.ExecuteWithErrorHandling(request);
    }

    public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider, Dictionary<string, string> values)
    {
        var allWebhooks = await GetAllWebhooks(Creds);

        var webhookToDelete = allWebhooks.FirstOrDefault(x => x.CallbackUrl == values["payloadUrl"]);
        if(webhookToDelete is null)
            return;
        
        var request = new BlackbirdAppRequest($"/webhooks/{webhookToDelete.Id}", Method.Delete, Creds);
        await Client.ExecuteWithErrorHandling(request);
    }

    private Task<IEnumerable<WebhookEntity>> GetAllWebhooks(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider)
    {
        var request = new BlackbirdAppRequest("/webhooks", Method.Get, authenticationCredentialsProvider);
        return Client.ExecuteWithErrorHandling<IEnumerable<WebhookEntity>>(request);
    }
}