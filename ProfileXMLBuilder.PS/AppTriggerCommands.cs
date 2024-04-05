using ProfileXMLBuilder.Lib;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace ProfileXMLBuilder.PS
{
    [Cmdlet(VerbsCommon.New, "ProfileXMLAppTrigger")]
    [OutputType(typeof(AppTrigger))]
    public class NewProfileXMLAppTriggerCommand : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string AppId { get; set; } = string.Empty;

        [Parameter(Mandatory = true)]
        public AppType AppType { get; set; } = AppType.FilePath;

        protected override void BeginProcessing()
        {
            var trigger = new AppTrigger()
            {
                App = new App()
                {
                    Id = AppId,
                    Type = AppType,
                }
            };
            WriteObject(trigger);
        }

        protected override void ProcessRecord()
        {
        }

        protected override void EndProcessing()
        {
        }
    }
}
