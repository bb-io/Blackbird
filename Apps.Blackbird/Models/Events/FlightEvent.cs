using Blackbird.Applications.Sdk.Common;

namespace Apps.Blackbird.Models.Events;
public class FlightEvent : BirdEvent
{
    [Display("Is Action skipped?")]
    public bool? IsSkipped { get; set; }

    [Display("Is Action retried?")]
    public bool? IsRetried { get; set; }

    [Display("Log level")]
    public string SdkLogType { get; set; }
}
