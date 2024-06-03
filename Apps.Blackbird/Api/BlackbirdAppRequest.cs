using Apps.Blackbird.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using RestSharp;

namespace Apps.Blackbird.Api;

public class BlackbirdAppRequest : BlackBirdRestRequest
{
    public BlackbirdAppRequest(string resource, Method method, IEnumerable<AuthenticationCredentialsProvider> creds) : base(resource, method, creds)
    {
    }

    protected override void AddAuth(IEnumerable<AuthenticationCredentialsProvider> creds)
    {
        this.AddHeader("Authorization", creds.Get(CredsNames.ApiToken).Value);
    }
}