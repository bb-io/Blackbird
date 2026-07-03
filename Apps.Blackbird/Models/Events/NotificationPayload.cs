using Blackbird.Applications.Sdk.Common;

namespace Apps.Blackbird.Models.Events;

public class NotificationPayload
{
    [Display("Notification ID")]
    public string Id { get; set; }

    [Display("Time")]
    public DateTime CreatedAt { get; set; }

    [Display("Nest ID")]
    public long WorkspaceId { get; set; }

    [Display("Bird ID")]
    public long? WorkflowId { get; set; }

    [Display("Bird name")]
    public string? BirdName { get; set; }

    [Display("App ID")]
    public long? AppId { get; set; }

    [Display("App plugin definition ID")]
    public long? AppPluginDefinitionId { get; set; }

    [Display("Connection name")]
    public string? ConnectionName { get; set; }

    [Display("Event type")]
    public string EventType { get; set; }

    [Display("Initiator")]
    public string? Initiator { get; set; }

    [Display("Message")]
    public string Message { get; set; }

    [Display("Disconnected by")]
    public string? DisconnectedBy { get; set; }
}
