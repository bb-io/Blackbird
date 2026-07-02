using Newtonsoft.Json;

namespace Apps.Blackbird.Models.Response;
public class LogResponse<T>(PaginatedResponse<T> raw)
{
    public List<T> Events { get; set; } = raw.Items;
    public string Log => JsonConvert.SerializeObject(Events, Formatting.Indented, new JsonSerializerSettings
    {
        NullValueHandling = NullValueHandling.Ignore
    });
}
