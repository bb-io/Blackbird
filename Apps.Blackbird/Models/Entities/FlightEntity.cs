using Blackbird.Applications.Sdk.Common;

namespace Apps.Blackbird.Models.Entities;

public class FlightEntity
{
    [Display("Flight ID")]
    public string Id { get; set; }
    
    [Display("Bird ID")]
    public string BirdId { get; set; }

    [Display("Nest ID")]
    public string NestId { get; set; }

    [Display("Status")]
    public string Status { get; set; }

    [Display("Error messages")]
    public string? ErrorMessages { get; set; }

    [Display("Start date")]
    public DateTime StartDate { get; set; }
    
    [Display("Duration")]
    public int Duration { get; set; }
}