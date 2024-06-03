using Apps.Blackbird.Api;
using Apps.Blackbird.Invocables;
using Apps.Blackbird.Models.Entities;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Blackbird.DataSourceHandlers;

public class NestDataSourceHandlers : BlackbirdAppInvocable, IAsyncDataSourceHandler
{
    public NestDataSourceHandlers(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var request = new BlackbirdAppRequest("nests", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<IEnumerable<NestEntity>>(request);

        return response
            .Where(x => string.IsNullOrEmpty(context.SearchString) ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(30)
            .ToDictionary(x => x.Id, x => x.Name);
    }
}