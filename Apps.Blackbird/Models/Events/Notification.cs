using Blackbird.Applications.Sdk.Common;

namespace Apps.Blackbird.Models.Events;
public class Notification
{
    [Display("Notification ID")]
    public string Id { get; set; }

    [Display("Time")]
    public DateTime CreatedAt { get; set; }

    [Display("Nest ID")]
    public string WorkSpaceId { get; set; }

    [Display("Entity ID")]
    public string EntityId { get; set; }

    [Display("Entity name")]
    public string EntityName { get; set; }

    [Display("Entity type")]
    public string EntityType { get; set; }

    [Display("Seen")]
    public bool? Seen { get; set; }

    [Display("Message")]
    public string Message { get; set; }
}
