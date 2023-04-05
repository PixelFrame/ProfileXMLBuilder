using ProfileXMLBuilder.Lib;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace ProfileXMLBuilder.PS
{
    [Cmdlet(VerbsCommon.New, "ProfileXMLRoute")]
    [OutputType(typeof(Route))]
    public class NewProfileXMLRoute : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Address { get; set; } = "0.0.0.0";

        [Parameter(Mandatory = true)]
        public byte PrefixSize { get; set; } = 0;

        [Parameter(Mandatory = false)]
        public SwitchParameter ExclusionRoute { get; set; }

        [Parameter(Mandatory = false)]
        public uint? Metric { get; set; } = null;

        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
            var route = new Route()
            {
                Address = Address,
                PrefixSize = PrefixSize,
                ExclusionRoute = ExclusionRoute.IsPresent ? ExclusionRoute : null,
                Metric = Metric
            };
            WriteObject(route);
        }

        protected override void EndProcessing()
        {
        }
    }
}
