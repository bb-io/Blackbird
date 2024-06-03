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
        var client = new BlackbirdAppClient(authenticationCredentialsProviders.ToArray());

        var request = new RestRequest("/users", Method.Post);
        await client.ExecuteWithErrorHandling(request);
        
        return new()
        {
            IsValid = true
        };
    }
}