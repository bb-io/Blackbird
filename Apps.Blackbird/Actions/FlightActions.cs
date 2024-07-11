using Apps.Blackbird.Api;
using Apps.Blackbird.Invocables;
using Apps.Blackbird.Models.Entities;
using Apps.Blackbird.Models.Request.Birds;
using Apps.Blackbird.Models.Request.Flights;
using Apps.Blackbird.Models.Response.Flights;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using RestSharp;

namespace Apps.Blackbird.Actions;

[ActionList]
public class FlightActions : BlackbirdAppInvocable
{
    public FlightActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }


    [Action("Search flights", Description = "Search for flights of the specific nest")]
    public async Task<ListFlightsResponse> ListFlights([ActionParameter] BirdRequest bird,
        [ActionParameter] ListFlightsRequest input)
    {
        var endpoint = $"nests/{bird.NestId}/birds/{bird.BirdId}/flights".WithQuery(input);
        var request = new BlackbirdAppRequest(endpoint, Method.Get, Creds);

        var response = await Client.ExecuteWithErrorHandling<IEnumerable<FlightEntity>>(request);
        return new()
        {
            Flights = response
        };
    }

    [Action("Get flight", Description = "Get details of a specific tenant flight")]
    public Task<FlightEntity> GetFlight([ActionParameter] FlightRequest flight)
    {
        var request = new BlackbirdAppRequest($"nests/{flight.NestId}/birds/{flight.BirdId}/flights/{flight.FlightId}",
            Method.Get, Creds);
        return Client.ExecuteWithErrorHandling<FlightEntity>(request);
    }
}