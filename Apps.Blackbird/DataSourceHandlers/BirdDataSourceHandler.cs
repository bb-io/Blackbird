using Apps.Blackbird.Api;
using Apps.Blackbird.Invocables;
using Apps.Blackbird.Models.Entities;
using Apps.Blackbird.Models.Request.Birds;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Blackbird.DataSourceHandlers;

public class BirdDataSourceHandler : BlackbirdAppInvocable, IAsyncDataSourceHandler
{
    private readonly BirdRequest _birdRequest;

    public BirdDataSourceHandler(InvocationContext invocationContext, [ActionParameter] BirdRequest birdRequest) : base(
        invocationContext)
    {
        _birdRequest = birdRequest;
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_birdRequest.NestId))
            throw new("You should input Nest ID first");

        var endpoint = $"nests/{_birdRequest.NestId}/birds";
        var request = new BlackbirdAppRequest(endpoint, Method.Get, Creds);

        var response = await Client.ExecuteWithErrorHandling<IEnumerable<BirdEntity>>(request);
        return response
            .Where(x => string.IsNullOrEmpty(context.SearchString) ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(30)
            .ToDictionary(x => x.Id, x => x.Name);
    }
}