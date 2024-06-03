using Apps.Blackbird.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Blackbird.Models.Request.Users;

public class UserRequest
{
    [Display("User ID")]
    [DataSource(typeof(UserDataSourceHandler))]
    public string UserId { get; set; }
}