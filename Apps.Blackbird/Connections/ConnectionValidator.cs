using Apps.Blackbird.Api;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;
using RestSharp;

namespace Apps.Blackbird.Connections;

public class ConnectionValidator: IConnectionValidator
{
    public async ValueTask<ConnectionValidationResponse> ValidateConnection(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        CancellationToken cancellationToken)
    {
        var creds = authenticationCredentialsProviders.ToArray();
        var client = new BlackbirdAppClient(creds);

        var request = new BlackbirdAppRequest("/users", Method.Get, creds);
        await client.ExecuteWithErrorHandling(request);
        
        return new()
        {
            IsValid = true
        };
    }
}