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
        [Parameter(Mandatory = false)]
        public string Servers { get; set; } = "vpn.contoso.com";

        [Parameter(Mandatory = false)]
        public string DnsSuffix { get; set; } = "contoso.com";

        [Parameter(Mandatory = false)]
        public string TrustedNetworkDetection { get; set; } = "contoso.com";

        [Parameter(Mandatory = false)]
        public RoutingPolicyType RoutingPolicy { get; set; } = RoutingPolicyType.SplitTunnel;

        [Parameter(Mandatory = false)]
        public NativeProtocolType NativeProtocol { get; set; } = NativeProtocolType.Automatic;

        [Parameter(Mandatory = false)]
        public SwitchParameter DeviceTunnel { get; set; }

        [Parameter(Mandatory = false)]
        public PSAuthentication Authentication { get; set; } = new();

        [Parameter(Mandatory = false)]
        public SwitchParameter DisableClassBasedDefaultRoutes { get; set; }

        [Parameter(Mandatory = false)]
        public DomainNameInformation[]? DomainNameInformation { get; set; }

        [Parameter(Mandatory = false)]
        public Route[]? Routes { get; set; }

        [Parameter(Mandatory = false)]
        public TrafficFilter[]? TrafficFilters { get; set; }

        [Parameter(Mandatory = false)]
        public AppTrigger[]? AppTriggers { get; set; }

        [Parameter(Mandatory = false)]
        public ProxyType ProxyType { get; set; }
        [Parameter(Mandatory = false)]
        public string? ProxyValue { get; set; }

        [Parameter(Mandatory = false)]
        public string? SsoEku { get; set; }
        [Parameter(Mandatory = false)]
        public string? SsoCA { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter NoRegisterDNS { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter NoRememberCredentials { get; set; }

        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
            var builder = new Builder()
                .SetServers(Servers)
                .SetDnsSuffix(DnsSuffix)
                .SetTrustedNetworkDetection(TrustedNetworkDetection)
                .SetRoutingPolicyType(RoutingPolicy)
                .SetNativeProtocolType(NativeProtocol)
                .SetDeviceTunnel(DeviceTunnel)
                .SetAuthentication(
                    AuthMethod: Authentication.AuthMethod,
                    RadiusServerNames: Authentication.RadiusServerNames,
                    RadiusServerRootCA: Authentication.RadiusServerRootCA,
                    DisableServerValidationPrompt: Authentication.DisableServerValidationPrompt,
                    CertSelectionCA: Authentication.CertSelectionCA,
                    AllPurposeEnabled: Authentication.AllPurposeEnabled,
                    CertSelectionEku: Authentication.CertSelectionEku)
                .SetDisableClassBasedDefaultRoute(DisableClassBasedDefaultRoutes);

            if (Routes != null) builder.AddRoutes(Routes);
            if (TrafficFilters != null) builder.AddTrafficFilters(TrafficFilters);
            if (DomainNameInformation != null) builder.AddDomainNameInformation(DomainNameInformation);
            if (AppTriggers != null) builder.AddAppTriggers(AppTriggers);
            if (ProxyValue != null) builder.SetProxy(ProxyType, ProxyValue);
            if (SsoEku != null || SsoCA != null) builder.SetDeviceCompliance(true, SsoEku, SsoCA);
            if (NoRegisterDNS) builder.SetRegisterDNS(false);
            if (NoRememberCredentials) builder.SetRememberCredentials(false);

            WriteObject(builder);
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
                    RadiusServerRootCA: ["00 11 22 33 44 55 66 77 88 99 aa bb cc dd ee ff 02 05 00 01 "],
                    DisableServerValidationPrompt: false,
                    CertSelectionCA: ["00 11 22 33 44 55 66 77 88 99 aa bb cc dd ee ff 02 05 00 01 "],
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
                    RadiusServerRootCA: ["00 11 22 33 44 55 66 77 88 99 aa bb cc dd ee ff 02 05 00 01 "],
                    DisableServerValidationPrompt: false,
                    CertSelectionCA: ["00 11 22 33 44 55 66 77 88 99 aa bb cc dd ee ff 02 05 00 01 "],
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
                    RadiusServerRootCA: ["00 11 22 33 44 55 66 77 88 99 aa bb cc dd ee ff 02 05 00 01 "],
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

    [Cmdlet(VerbsCommon.Get, "ProfileXML")]
    [OutputType(typeof(string))]
    public class GetProfileXMLCommand : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public Builder? Builder { get; set; }

        protected override void BeginProcessing()
        {
        }

        protected override void ProcessRecord()
        {
            var xml = Builder!.GetXml();
            WriteObject(xml);
        }

        protected override void EndProcessing()
        {
        }
    }
}
