using Apps.Blackbird.DataSourceHandlers.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Blackbird.Models.Request.Flights;

public class ListFlightsRequest
{
    [Display("Minimum start date")]
    public DateTime? MinStartDate { get; set; }
    
    [Display("Maximum start date")]
    public DateTime? MaxStartDate { get; set; }
    
    [StaticDataSource(typeof(FlightStatusDataSourceHandler))]
    public string? Status { get; set; }
}