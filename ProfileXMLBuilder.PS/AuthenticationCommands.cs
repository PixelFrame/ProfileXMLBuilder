using ProfileXMLBuilder.Lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace ProfileXMLBuilder.PS
{
    [Cmdlet(VerbsCommon.New, "ProfileXMLAuthentication")]
    [OutputType(typeof(PSAuthentication))]
    public class NewProfileXMLAuthentication : PSCmdlet
    {
        [Parameter(Position = 0, ParameterSetName = "Machine")]
        public SwitchParameter MachineAuth { get; set; }

        [Parameter(Position = 0, ParameterSetName = "User")]
        public SwitchParameter UserAuth { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Machine")]
        [ValidateSet("MachineCert")]
        public AuthenticationMethod ComputerMethod { get; set; } = AuthenticationMethod.MachineCert;

        [Parameter(Mandatory = true, ParameterSetName = "User")]
        [ValidateSet("UserMschapv2", "UserEapTls", "UserEapMschapv2", "UserPeapTls", "UserPeapMschapv2")]
        public AuthenticationMethod UserMethod { get; set; } = AuthenticationMethod.UserMschapv2;

        [Parameter(Mandatory = false, ParameterSetName = "User")]
        public string? RadiusServerNames { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = "User")]
        public string[]? RadiusServerRootCA { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = "User")]
        public SwitchParameter DisableServerValidationPrompt { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = "User")]
        public string[]? CertSelectionCA { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = "User")]
        public Hashtable? CertSelectionEku { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = "User")]
        public SwitchParameter CertSelectionAllPurposeEnabled { get; set; }

        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
            var auth = new PSAuthentication();
            if (MachineAuth)
            {
                auth.AuthMethod = AuthenticationMethod.MachineCert;
            }
            else if (UserMethod == AuthenticationMethod.UserMschapv2)
            {
                auth.AuthMethod = AuthenticationMethod.UserMschapv2;
            }
            else
            {
                auth.AuthMethod = UserMethod;
                auth.RadiusServerNames = RadiusServerNames;
                auth.RadiusServerRootCA = RadiusServerRootCA == null ? null : new(RadiusServerRootCA);
                auth.DisableServerValidationPrompt = DisableServerValidationPrompt.IsPresent ? DisableServerValidationPrompt : null;
                auth.CertSelectionCA = CertSelectionCA == null ? null : new(CertSelectionCA);
                auth.AllPurposeEnabled = CertSelectionAllPurposeEnabled.IsPresent ? CertSelectionAllPurposeEnabled : null;
                if (CertSelectionEku != null)
                {
                    auth.CertSelectionEku = new();
                    foreach (DictionaryEntry _ in CertSelectionEku)
                    {
                        auth.CertSelectionEku.Add(new(_.Key.ToString(), _.Value.ToString()));
                    }
                }
            }
            WriteObject(auth);
        }

        protected override void EndProcessing()
        {
        }
    }

    public class PSAuthentication
    {
        public AuthenticationMethod AuthMethod;
        public string? RadiusServerNames;
        public List<string>? RadiusServerRootCA;
        public bool? DisableServerValidationPrompt;
        public List<string>? CertSelectionCA;
        public bool? AllPurposeEnabled;
        public List<KeyValuePair<string, string>>? CertSelectionEku;
    }
}
