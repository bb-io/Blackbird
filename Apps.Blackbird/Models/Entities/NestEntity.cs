using Blackbird.Applications.Sdk.Common;

namespace Apps.Blackbird.Models.Entities;

public class NestEntity
{
    [Display("Nest ID")]
    public string Id { get; set; }

    [Display("Nest name")]
    public string Name { get; set; }

    [Display("Members")]
    public IEnumerable<UserEntity> Members { get; set; }
}