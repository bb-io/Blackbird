using Blackbird.Applications.Sdk.Common;

namespace Apps.Blackbird.Models.Response.Flights;

public class OtherActiveFlightsResponse
{
    [Display("Has other flights")]
    public bool HasOtherFlights { get; set; }

    [Display("Other flight IDs")]
    public IEnumerable<string> OtherFlightIds { get; set; } = [];
}
