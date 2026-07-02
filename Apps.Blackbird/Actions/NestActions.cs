using Apps.Blackbird.Api;
using Apps.Blackbird.Invocables;
using Apps.Blackbird.Models.Entities;
using Apps.Blackbird.Models.Request.Nests;
using Apps.Blackbird.Models.Request.Users;
using Apps.Blackbird.Models.Response.Nests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Blackbird.Actions;

[ActionList("Nests")]
public class NestActions : BlackbirdAppInvocable
{
    public NestActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("Search Nests", Description = "Returns a list of all Nests")]
    public async Task<ListNestsResponse> ListNests()
    {
        var request = new BlackbirdAppRequest("nests", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<IEnumerable<NestEntity>>(request);

        return new()
        {
            Nests = response
        };
    }

    [Action("Get Nest", Description = "Get details of a specific Nest")]
    public Task<NestEntity> GetNest([ActionParameter] NestRequest nest)
    {
        var request = new BlackbirdAppRequest($"nests/{nest.NestId}", Method.Get, Creds);
        return Client.ExecuteWithErrorHandling<NestEntity>(request);
    }

    [Action("Add user to Nest", Description = "Add a new user to the specific Nest")]
    public Task AddUserToNest([ActionParameter] NestRequest nest, [ActionParameter] UserRequest user)
    {
        var request = new BlackbirdAppRequest($"nests/{nest.NestId}/users", Method.Put, Creds)
            .WithJsonBody(new
            {
                user.UserId
            });
        return Client.ExecuteWithErrorHandling(request);
    }

    [Action("Remove user from Nest", Description = "Remove specific user from the Nest")]
    public Task RemoveUserFromNest([ActionParameter] NestRequest nest, [ActionParameter] UserRequest user)
    {
        var request = new BlackbirdAppRequest($"nests/{nest.NestId}/users/{user.UserId}", Method.Delete, Creds);
        return Client.ExecuteWithErrorHandling(request);
    }
}