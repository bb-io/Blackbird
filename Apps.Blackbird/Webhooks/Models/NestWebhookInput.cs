using Apps.Blackbird.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Blackbird.Webhooks.Models;

public class NestWebhookInput
{
    [Display("Nest ID")]
    [DataSource(typeof(NestDataSourceHandlers))]
    public string? NestId { get; set; }
}