using Apps.Blackbird.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Blackbird.Models.Request.Nests;

public class NestRequest
{
    [Display("Nest ID")]
    [DataSource(typeof(NestDataSourceHandlers))]
    public string NestId { get; set; }
}