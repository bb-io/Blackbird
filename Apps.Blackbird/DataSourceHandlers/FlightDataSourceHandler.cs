using Apps.Blackbird.Api;
using Apps.Blackbird.Invocables;
using Apps.Blackbird.Models.Entities;
using Apps.Blackbird.Models.Request.Flights;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Blackbird.DataSourceHandlers;

public class FlightDataSourceHandler: BlackbirdAppInvocable, IAsyncDataSourceHandler
{
    private readonly FlightRequest _flightRequest;

    public FlightDataSourceHandler(InvocationContext invocationContext, [ActionParameter] FlightRequest birdRequest) : base(
        invocationContext)
    {
        _flightRequest = birdRequest;
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_flightRequest.NestId))
            throw new("You should input Nest ID first");
        
        if (string.IsNullOrEmpty(_flightRequest.BirdId))
            throw new("You should input Bird ID first");


        var endpoint = $"nests/{_flightRequest.NestId}/birds/{_flightRequest.BirdId}/flights";
        var request = new BlackbirdAppRequest(endpoint, Method.Get, Creds);

        var response = await Client.ExecuteWithErrorHandling<IEnumerable<FlightEntity>>(request);
        return response
            .Select(x => (x.Id, $"{x.Id}: {x.Status}"))
            .Where(x => string.IsNullOrEmpty(context.SearchString) ||
                        x.Item2.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(30)
            .ToDictionary(x => x.Id, x => x.Item2);
    }
}