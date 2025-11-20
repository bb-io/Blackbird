using Apps.Blackbird.Constants;
using Apps.Blackbird.Models.Response;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using RestSharp;
using System.Text.RegularExpressions;

namespace Apps.Blackbird.Api;

public class BlackbirdAppClient : BlackBirdRestClient
{
    public BlackbirdAppClient(AuthenticationCredentialsProvider[] creds) : base(new()
    {
        BaseUrl = (creds.Get(CredsNames.Url).Value.TrimEnd('/') + "/api-rest").ToUri()
    })
    {
    }

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        string? detail = null;

        if (!string.IsNullOrWhiteSpace(response.Content))
        {
            if (response.ContentType == "application/json")
            {
                var error = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
                detail = string.IsNullOrWhiteSpace(error?.Detail) ? null : error.Detail;
            }
            else if (response.ContentType == "text/html")
            {
                var match = Regex.Match(response.Content, @"<title>(.*?)</title>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                detail = match.Success ? match.Groups[1].Value.Trim() : null;
            }
        }

        detail ??= !string.IsNullOrWhiteSpace(response.StatusDescription)
            ? response.StatusDescription
            : response.Content;

        var message = $"Status {response.StatusCode} - {detail}";
        return new PluginApplicationException(message);
    }
}