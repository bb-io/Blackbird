using Apps.Blackbird.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Blackbird.Webhooks.Models
{
    public class BirdWebhookRequest
    {
        [Display("Nest ID")]
        [DataSource(typeof(NestDataSourceHandlers))]
        public string? NestId { get; set; }

        [Display("Bird ID")]
        [DataSource(typeof(BirdDataSourceHandler))]
        public string? BirdId { get; set; }
    }
}
