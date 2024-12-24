using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Blackbird.Models.Entities;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Blackbird.Webhooks.Models
{
    public class FlightWrapperResponse
    {
        [Display("Flight ID")]
        public string Id { get; set; }

        [Display("Bird")]
        public BirdEntity Bird { get; set; }

        [Display("Nest")]
        public NestEntity Nest { get; set; }

        [Display("Status")]
        public string Status { get; set; }

        [Display("Error messages")]
        public string? ErrorMessages { get; set; }

        [Display("Start date")]
        public DateTime StartDate { get; set; }

        [Display("Duration")]
        public int Duration { get; set; }
    }
}
