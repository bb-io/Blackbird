using Apps.Blackbird.DataSourceHandlers.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Blackbird.Models.Request.Birds;

public class ListBirdsRequest
{
    [Display("Trigger type")]
    [StaticDataSource(typeof(BirdTriggerTypeDataHandler))]
    public string? TriggerType { get; set; }
    
    [StaticDataSource(typeof(BirdStatusDataHandler))]
    public string? Status { get; set; }
}