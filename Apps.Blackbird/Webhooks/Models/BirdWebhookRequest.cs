using Apps.Blackbird.DataSourceHandlers;
using Apps.Blackbird.DataSourceHandlers.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
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

        [Display("Bird status")]
        [StaticDataSource(typeof(BirdStatusDataHandler))]
        public string? BirdStatus { get; set; }
    }
}
