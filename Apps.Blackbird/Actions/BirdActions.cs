using Apps.Blackbird.Api;
using Apps.Blackbird.Invocables;
using Apps.Blackbird.Models.Entities;
using Apps.Blackbird.Models.Events;
using Apps.Blackbird.Models.Request.Birds;
using Apps.Blackbird.Models.Request.Nests;
using Apps.Blackbird.Models.Response;
using Apps.Blackbird.Models.Response.Birds;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using RestSharp;

namespace Apps.Blackbird.Actions;

[ActionList("Birds")]
public class BirdActions : BlackbirdAppInvocable
{
    public BirdActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    [Action("Search Birds", Description = "Searches for birds in a specific nest")]
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

    [Action("Get Bird", Description = "Gets details about a specific bird")]
    public Task<BirdEntity> GetBird([ActionParameter] BirdRequest bird)
    {
        return GetBirdDetails(bird.NestId, bird.BirdId);
    }

    [Action("Get Bird logs", Description = "Gets logs for a specific bird")]
    public async Task<LogResponse<BirdEvent>> GetBirdLogs([ActionParameter] BirdRequest bird)
    {
        var request = new BlackbirdAppRequest($"nests/{bird.NestId}/birds/{bird.BirdId}/logs", Method.Get, Creds);
        request.AddQueryParameter("pageSize", 100);
        var response = await Client.ExecuteWithErrorHandling<PaginatedResponse<BirdEvent>>(request);

        return new(response);
    }

    [Action("Fly Bird", Description = "Starts a flight for a published bird with a manual trigger")]
    public async Task StartBird([ActionParameter] StartBirdRequest bird)
    {
        var birdDetails = await GetBirdDetails(bird.NestId, bird.BirdId);

        if (!string.Equals(birdDetails.TriggerType, "manual", StringComparison.OrdinalIgnoreCase))
        {
            throw new PluginMisconfigurationException($"Only Birds with manual triggers can be started through this action");
        }

        var request = new BlackbirdAppRequest($"nests/{bird.NestId}/birds/{bird.BirdId}", Method.Post, Creds);
        await Client.ExecuteWithErrorHandling(request);
    }

    private async Task<BirdEntity> GetBirdDetails(string nestId, string birdId)
    {
        var request = new BlackbirdAppRequest($"nests/{nestId}/birds/{birdId}", Method.Get, Creds);
        return await Client.ExecuteWithErrorHandling<BirdEntity>(request);
    }
}
