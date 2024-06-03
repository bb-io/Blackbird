using Blackbird.Applications.Sdk.Common;

namespace Apps.Blackbird.Models.Entities;

public class UserEntity
{
    [Display("User ID")]
    public string Id { get; set; }

    [Display("First name")]
    public string FirstName { get; set; }

    [Display("Last name")]
    public string LastName { get; set; }

    [Display("Email")]
    public string Email { get; set; }

    [Display("Is active")]
    public bool IsActive { get; set; }
}