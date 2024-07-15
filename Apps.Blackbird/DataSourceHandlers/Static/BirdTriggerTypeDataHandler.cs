using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Blackbird.DataSourceHandlers.Static;

public class BirdTriggerTypeDataHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData()
    {
        return new()
        {
            ["manual"] = "Manual",
            ["scheduled"] = "Scheduled",
            ["event"] = "Event",
        };
    }
}