using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Blackbird.DataSourceHandlers.Static;

public class FlightStatusDataSourceHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData()
    {
        return new()
        {
            ["succeeded"] = "Succeeded",
            ["failed"] = "Failed",
            ["cancelled"] = "Cancelled",
            ["active"] = "Active",
        };
    }
}