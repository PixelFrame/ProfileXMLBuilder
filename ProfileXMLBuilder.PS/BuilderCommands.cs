using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

using ProfileXMLBuilder.Lib;

namespace ProfileXMLBuilder.PS
{
    [Cmdlet(VerbsCommon.New, "ProfileXMLBuilder")]
    [OutputType(typeof(Builder))]
    public class NewProfileXMLBuilderCommand : PSCmdlet
    {
        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
            WriteObject(new Builder());
        }

        protected override void EndProcessing()
        {
        }
    }

    [Cmdlet(VerbsCommon.New, "SampleUTPeapTlsBuilder")]
    [OutputType(typeof(Builder))]
    public class NewSampleUTPeapTlsBuilderCommand : PSCmdlet
    {
        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
            var builder = new Builder()
                .SetAuthentication(
                    AuthMethod: AuthenticationMethod.UserPeapTls,
                    RadiusServerNames: "nps.contoso.com",
                    RadiusServerRootCA: new() { "00 11 22 33 44 55 66 77 88 99 aa bb cc dd ee ff 02 05 00 01 " },
                    DisableServerValidationPrompt: false,
                    CertSelectionCA: new() { "00 11 22 33 44 55 66 77 88 99 aa bb cc dd ee ff 02 05 00 01 " },
                    AllPurposeEnabled: true,
                    CertSelectionEku: null)
                .AddDomainNameInformation(".contoso.com", "10.1.1.1", null, null, null)
                .AddDomainNameInformation("contoso.com", "10.1.1.1", null, null, null)
                .AddRoute("10.1.1.0", 24, null, null);
            WriteObject(builder);
        }

        protected override void EndProcessing()
        {
        }
    }

    [Cmdlet(VerbsCommon.New, "SampleUTEapTlsBuilder")]
    [OutputType(typeof(Builder))]
    public class NewSampleUTEapTlsBuilderCommand : PSCmdlet
    {
        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
            var builder = new Builder()
                .SetAuthentication(
                    AuthMethod: AuthenticationMethod.UserEapTls,
                    RadiusServerNames: "nps.contoso.com",
                    RadiusServerRootCA: new() { "00 11 22 33 44 55 66 77 88 99 aa bb cc dd ee ff 02 05 00 01 " },
                    DisableServerValidationPrompt: false,
                    CertSelectionCA: new() { "00 11 22 33 44 55 66 77 88 99 aa bb cc dd ee ff 02 05 00 01 " },
                    AllPurposeEnabled: true,
                    CertSelectionEku: null)
                .AddDomainNameInformation(".contoso.com", "10.1.1.1", null, null, null)
                .AddDomainNameInformation("contoso.com", "10.1.1.1", null, null, null)
                .AddRoute("10.1.1.0", 24, null, null);
            WriteObject(builder);
        }

        protected override void EndProcessing()
        {
        }
    }

    [Cmdlet(VerbsCommon.New, "SampleUTEapMschapv2Builder")]
    [OutputType(typeof(Builder))]
    public class NewSampleUTEapMschapv2BuilderCommand : PSCmdlet
    {
        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
            var builder = new Builder()
                .SetAuthentication(
                    AuthMethod: AuthenticationMethod.UserEapMschapv2,
                    RadiusServerNames: null,
                    RadiusServerRootCA: null,
                    DisableServerValidationPrompt: null,
                    CertSelectionCA: null,
                    AllPurposeEnabled: null,
                    CertSelectionEku: null)
                .AddDomainNameInformation(".contoso.com", "10.1.1.1", null, null, null)
                .AddDomainNameInformation("contoso.com", "10.1.1.1", null, null, null)
                .AddRoute("10.1.1.0", 24, null, null);
            WriteObject(builder);
        }

        protected override void EndProcessing()
        {
        }
    }

    [Cmdlet(VerbsCommon.New, "SampleUTPeapMschapv2Builder")]
    [OutputType(typeof(Builder))]
    public class NewSampleUTPeapMschapv2BuilderCommand : PSCmdlet
    {
        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
            var builder = new Builder()
                .SetAuthentication(
                    AuthMethod: AuthenticationMethod.UserPeapMschapv2,
                    RadiusServerNames: "nps.contoso.com",
                    RadiusServerRootCA: new() { "00 11 22 33 44 55 66 77 88 99 aa bb cc dd ee ff 02 05 00 01 " },
                    DisableServerValidationPrompt: false,
                    CertSelectionCA: null,
                    AllPurposeEnabled: null,
                    CertSelectionEku: null)
                .AddDomainNameInformation(".contoso.com", "10.1.1.1", null, null, null)
                .AddDomainNameInformation("contoso.com", "10.1.1.1", null, null, null)
                .AddRoute("10.1.1.0", 24, null, null);
            WriteObject(builder);
        }

        protected override void EndProcessing()
        {
        }
    }

    [Cmdlet(VerbsCommon.New, "SampleDTBuilder")]
    [OutputType(typeof(Builder))]
    public class NewSampleDTBuilderCommand : PSCmdlet
    {
        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
            var builder = new Builder()
                .SetDeviceTunnel(true)
                .SetNativeProtocolType(NativeProtocolType.IKEv2)
                .SetAuthentication(AuthenticationMethod.MachineCert, null, null, null, null, null, null)
                .AddRoute("10.1.1.0", 24, null, null);
            WriteObject(builder);
        }

        protected override void EndProcessing()
        {
        }
    }
}
