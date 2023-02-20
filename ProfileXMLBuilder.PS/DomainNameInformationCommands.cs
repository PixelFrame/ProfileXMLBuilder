using ProfileXMLBuilder.Lib;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Net;
using System.Text;

namespace ProfileXMLBuilder.PS
{
    [Cmdlet(VerbsCommon.New, "ProfileXMLDomainNameInformation")]
    [OutputType(typeof(DomainNameInformation))]
    public class NewProfileXMLDomainNameInformation : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string DomainName { get; set; } = "contoso.com";

        [Parameter(Mandatory = false)]
        public string? DnsServers { get; set; } = null;

        [Parameter(Mandatory = false)]
        public string? WebProxyServers { get; set; } = null;

        [Parameter(Mandatory = false)]
        public SwitchParameter AutoTrigger { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter Persistent { get; set; }

        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
            WriteObject(new DomainNameInformation()
            {
                DomainName = DomainName,
                DnsServers = DnsServers,
                WebProxyServers = WebProxyServers,
                AutoTrigger = AutoTrigger.IsPresent ? AutoTrigger : null,
                Persistent = Persistent.IsPresent ? Persistent : null
            });
        }

        protected override void EndProcessing()
        {
        }
    }
}
