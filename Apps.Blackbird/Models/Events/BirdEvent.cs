using Blackbird.Applications.Sdk.Common;

namespace Apps.Blackbird.Models.Events;
public class BirdEvent
{
    [Display("Event ID")]
    public string Id { get; set; }

    [Display("Time")]
    public DateTime CreatedAt { get; set; }

    [Display("Event type")]
    public string EventType { get; set; }

    [Display("Message")]
    public string Message { get; set; }

    [Display("Error source")]
    public string EventFailedBy { get; set; }

    [Display("Error Type")]
    public string ErrorType { get; set; }

    [Display("User name")]
    public string Initiator { get; set; }
}
