using Blackbird.Applications.Sdk.Common;

namespace Apps.Blackbird.Models.Entities;

public class BirdEntity
{
    [Display("Bird ID")]
    public string Id { get; set; }

    [Display("Nest ID")]
    public string NestId { get; set; }

    [Display("Name")]
    public string Name { get; set; }

    [Display("Status")]
    public string Status { get; set; }

    [Display("Trigger type")]
    public string TriggerType { get; set; }

    [Display("Is published")]
    public bool IsPublished { get; set; }
}