using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Blackbird.DataSourceHandlers.Static;

public class BirdStatusDataHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData()
    {
        return new()
        {
            ["active"] = "Active",
            ["unpublished"] = "Unpublished",
            ["suspended"] = "Suspended",
        };
    }
}