using Apps.Blackbird.Constants;
using Apps.Blackbird.Models.Response;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Blackbird.Api;

public class BlackbirdAppClient : BlackBirdRestClient
{
    public BlackbirdAppClient(AuthenticationCredentialsProvider[] creds) : base(new()
    {
        BaseUrl = creds.Get(CredsNames.Url).Value.ToUri()
    })
    {
    }

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        var error = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
        return new(error.Detail);
    }
}