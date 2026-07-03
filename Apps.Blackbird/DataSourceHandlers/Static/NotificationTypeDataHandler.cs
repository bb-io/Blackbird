using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Blackbird.DataSourceHandlers.Static;

public class NotificationTypeDataHandler : IStaticDataSourceItemHandler
{
    IEnumerable<DataSourceItem> IStaticDataSourceItemHandler.GetData()
    {
        return new List<DataSourceItem> 
        {
            new DataSourceItem("all", "All"),
            new DataSourceItem("ConnectionDisconnected", "Disconnected connections"),
            new DataSourceItem("FlowDeactivated", "Bird deactivated")
        };
    }
}