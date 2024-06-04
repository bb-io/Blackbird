using Apps.Blackbird.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Blackbird.Models.Request.Flights;

public class FlightRequest
{
    [Display("Nest ID")]
    [DataSource(typeof(NestDataSourceHandlers))]
    public string NestId { get; set; }

    [Display("Bird ID")]
    [DataSource(typeof(BirdFlightDataSourceHandler))]
    public string BirdId { get; set; }
    
    [Display("Flight ID")]
    [DataSource(typeof(FlightDataSourceHandler))]
    public string FlightId { get; set; }
}