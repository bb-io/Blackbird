using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Blackbird.Models.Entities;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Blackbird.Webhooks.Models
{
    public class BirdWrapperResponse
    {
        [Display("Bird ID")]
        public string Id { get; set; }

        [Display("Nest")]
        public NestEntity Nest { get; set; }

        [Display("Name")]
        public string Name { get; set; }

        [Display("Status")]
        public string Status { get; set; }

        [Display("Trigger type")]
        public string TriggerType { get; set; }

        [Display("Is published")]
        public bool IsPublished { get; set; }
    }
}
