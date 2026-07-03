using Apps.Blackbird.DataSourceHandlers.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Blackbird.Webhooks.Models;
public class NotificationTypeRequest
{
    [Display("Notification type")]
    [StaticDataSource(typeof(NotificationTypeDataHandler))]
    public string? EventType { get; set; }
}
