using Apps.Blackbird.Models.Entities;

namespace Apps.Blackbird.Models.Response.Flights;

public class ListFlightsResponse
{
    public IEnumerable<FlightEntity> Flights { get; set; }
}