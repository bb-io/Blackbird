using Apps.Blackbird.Api;
using Apps.Blackbird.Invocables;
using Apps.Blackbird.Models.Entities;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Blackbird.DataSourceHandlers;

public class UserDataSourceHandler : BlackbirdAppInvocable, IAsyncDataSourceHandler
{
    public UserDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var request = new BlackbirdAppRequest("users", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<IEnumerable<UserEntity>>(request);

        return response
            .Select(x => (x.Id, $"{x.FirstName} {x.LastName}"))
            .Where(x => string.IsNullOrEmpty(context.SearchString) ||
                        x.Item2.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(30)
            .ToDictionary(x => x.Id, x => x.Item2);
    }
}