using Apps.Blackbird.Api;
using Apps.Blackbird.Invocables;
using Apps.Blackbird.Models.Events;
using Apps.Blackbird.Models.Response;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Blackbird.Actions;

[ActionList("Admin")]
public class AdminActions : BlackbirdAppInvocable
{
    public AdminActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }


    [Action("Get latest notifications", Description = "Get all the latest notifications across all nests")]
    public async Task<LogResponse<Notification>> GetNotifications()
    {
        var endpoint = $"notifications";
        var request = new BlackbirdAppRequest(endpoint, Method.Get, Creds);

        var response = await Client.ExecuteWithErrorHandling<PaginatedResponse<Notification>>(request);
        return new(response);
    }
}
