using Apps.Blackbird.Models.Entities;

namespace Apps.Blackbird.Models.Response.Nests;

public class ListNestsResponse
{
    public IEnumerable<NestEntity> Nests { get; set; }
}