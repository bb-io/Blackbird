using Apps.Blackbird.Api;
using Apps.Blackbird.Invocables;
using Apps.Blackbird.Models.Entities;
using Apps.Blackbird.Models.Request.Birds;
using Apps.Blackbird.Models.Request.Nests;
using Apps.Blackbird.Models.Response.Birds;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using RestSharp;

namespace Apps.Blackbird.Actions;

[ActionList]
public class BirdActions : BlackbirdAppInvocable
{
    public BirdActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("Search birds", Description = "Search for birds of the specific nest")]
    public async Task<ListBirdsResponse> ListBirds([ActionParameter] NestRequest nest,
        [ActionParameter] ListBirdsRequest input)
    {
        var endpoint = $"nests/{nest.NestId}/birds".WithQuery(input);
        var request = new BlackbirdAppRequest(endpoint, Method.Get, Creds);

        var response = await Client.ExecuteWithErrorHandling<IEnumerable<BirdEntity>>(request);
        return new()
        {
            Birds = response
        };
    }

    [Action("Get bird", Description = "Get details of a specific tenant bird")]
    public Task<BirdEntity> GetBird([ActionParameter] BirdRequest bird)
    {
        var request = new BlackbirdAppRequest($"nests/{bird.NestId}/birds/{bird.BirdId}", Method.Get, Creds);
        return Client.ExecuteWithErrorHandling<BirdEntity>(request);
    }   
    
    [Action("Start bird", Description = "Start specific published bird")]
    public Task StartBird([ActionParameter] StartBirdRequest bird)
    {
        var request = new BlackbirdAppRequest($"nests/{bird.NestId}/birds/{bird.BirdId}", Method.Post, Creds);
        return Client.ExecuteWithErrorHandling(request);
    }
}