using Apps.Blackbird.Api;
using Apps.Blackbird.Invocables;
using Apps.Blackbird.Models.Entities;
using Apps.Blackbird.Models.Events;
using Apps.Blackbird.Models.Request.Birds;
using Apps.Blackbird.Models.Request.Flights;
using Apps.Blackbird.Models.Response;
using Apps.Blackbird.Models.Response.Flights;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using RestSharp;

namespace Apps.Blackbird.Actions;

[ActionList("Flights")]
public class FlightActions : BlackbirdAppInvocable
{
    public FlightActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }


    [Action("Search Flights", Description = "Searches for flights belonging to a specific bird")]
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

    [Action("Search other active flights", Description = "Checks whether the current bird has other active flights and outputs their IDs")]
    public async Task<OtherActiveFlightsResponse> CheckForOtherActiveFlights()
    {
        var nestId = InvocationContext.Workspace?.Id.ToString();
        var birdId = InvocationContext.Bird?.Id.ToString();
        var flightId = InvocationContext.Flight?.Id;

        if (nestId is null || birdId is null || flightId is null)
            throw new PluginApplicationException("Current Nest, Bird, and Flight context is required.");

        var response = await ListFlights(
            new BirdRequest { NestId = nestId, BirdId = birdId },
            new ListFlightsRequest { Status = "active" });

        var otherFlightIds = response.Flights
            .Where(flight => !string.Equals(flight.Id, flightId, StringComparison.OrdinalIgnoreCase))
            .Select(flight => flight.Id)
            .ToArray();

        return new()
        {
            HasOtherFlights = otherFlightIds.Length > 0,
            OtherFlightIds = otherFlightIds
        };
    }

    [Action("Get Flight", Description = "Gets details about a specific flight")]
    public Task<FlightEntity> GetFlight([ActionParameter] FlightRequest flight)
    {
        var request = new BlackbirdAppRequest($"nests/{flight.NestId}/birds/{flight.BirdId}/flights/{flight.FlightId}",
            Method.Get, Creds);
        return Client.ExecuteWithErrorHandling<FlightEntity>(request);
    }

    [Action("Get Flight Logs", Description = "Gets logs for a specific flight")]
    public async Task<LogResponse<FlightEvent>> GetFlightLogs([ActionParameter] FlightRequest flight)
    {
        var request = new BlackbirdAppRequest($"nests/{flight.NestId}/birds/{flight.BirdId}/flights/{flight.FlightId}/logs", Method.Get, Creds);
        request.AddQueryParameter("pageSize", 100);
        var response = await Client.ExecuteWithErrorHandling<PaginatedResponse<FlightEvent>>(request);

        return new(response);
    }
}
