using Apps.Blackbird.Api;
using Apps.Blackbird.Invocables;
using Apps.Blackbird.Models.Entities;
using Apps.Blackbird.Models.Request.Users;
using Apps.Blackbird.Models.Response.Users;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Blackbird.Actions;

[ActionList]
public class UserActions : BlackbirdAppInvocable
{
    public UserActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("Search users", Description = "Search for users of the tenant")]
    public async Task<ListUsersResponse> ListUsers()
    {
        var request = new BlackbirdAppRequest("users", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<IEnumerable<UserEntity>>(request);

        return new()
        {
            Users = response
        };
    }
    
    [Action("Get user", Description = "Get details of a specific tenant user")]
    public Task<UserEntity> GetUser([ActionParameter] UserRequest user)
    {
        var request = new BlackbirdAppRequest($"users/{user.UserId}", Method.Get, Creds);
        return Client.ExecuteWithErrorHandling<UserEntity>(request);
    }
}