using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Metadata;

namespace Apps.Blackbird;

public class Application : IApplication, ICategoryProvider
{
    public IEnumerable<ApplicationCategory> Categories
    {
        get => [ApplicationCategory.Utilities];
        set { }
    }

    public string Name
    {
        get => "Blackbird";
        set { }
    }

    public T GetInstance<T>()
    {
        throw new NotImplementedException();
    }
}