using ProfileXMLBuilder.Lib;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace ProfileXMLBuilder.PS
{
    [Cmdlet(VerbsCommon.New, "ProfileXMLTrafficFilter")]
    [OutputType(typeof(TrafficFilter))]
    public class NewProfileXMLTrafficFilterCommand : PSCmdlet
    {
        [Parameter(Mandatory = false)]
        public string? AppId { get; set; } = null;

        [Parameter(Mandatory = false)]
        public string? Protocol { get; set; } = null;

        [Parameter(Mandatory = false)]
        public string? LocalPortRanges { get; set; } = null;

        [Parameter(Mandatory = false)]
        public string? RemotePortRanges { get; set; } = null;

        [Parameter(Mandatory = false)]
        public string? LocalAddressRanges { get; set; } = null;

        [Parameter(Mandatory = false)]
        public string? RemoteAddressRanges { get; set; } = null;

        [Parameter(Mandatory = false)]
        public RoutingPolicyType? RoutingPolicyType { get; set; } = null;

        [Parameter(Mandatory = false)]
        public string? Direction { get; set; } = null;

        protected override void BeginProcessing()
        {
            var filter = new TrafficFilter()
            {
                Protocol = Protocol,
                LocalPortRanges = LocalPortRanges,
                RemotePortRanges = RemotePortRanges,
                LocalAddressRanges = LocalAddressRanges,
                RemoteAddressRanges = RemoteAddressRanges,
                RoutingPolicyType = RoutingPolicyType?.ToString(),
                Direction = Direction
            };
            if (AppId != null)
            {
                filter.App = new()
                {
                    Id = AppId
                };
            }
            WriteObject(filter);
        }

        protected override void ProcessRecord()
        {
        }

        protected override void EndProcessing()
        {
        }
    }
}
