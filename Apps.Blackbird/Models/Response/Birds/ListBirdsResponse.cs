using Apps.Blackbird.Models.Entities;

namespace Apps.Blackbird.Models.Response.Birds;

public class ListBirdsResponse
{
    public IEnumerable<BirdEntity> Birds { get; set; }
}