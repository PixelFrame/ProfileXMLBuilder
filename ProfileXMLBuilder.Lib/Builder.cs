using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace ProfileXMLBuilder.Lib
{
    public class Builder
    {
        private readonly VPNProfile _profile = new();

        public Builder()
        { }

        public Builder SetRememberCredentials(bool? Value)
        {
            _profile.RememberCredentials = Value;
            return this;
        }

        public Builder SetDnsSuffix(string? Value)
        {
            _profile.DnsSuffix = Value;
            return this;
        }

        public Builder SetRegisterDNS(bool? Value)
        {
            _profile.RegisterDNS = Value;
            return this;
        }

        public Builder SetTrustedNetworkDetection(string? Value)
        {
            _profile.TrustedNetworkDetection = Value;
            return this;
        }

        public Builder SetAlwaysOn(bool? Value)
        {
            _profile.AlwaysOn = Value;
            return this;
        }

        public Builder SetDeviceTunnel(bool? Value)
        {
            _profile.DeviceTunnel = Value;
            return this;
        }

        public Builder SetProxy(ProxyType Type, string Value)
        {
            switch (Type)
            {
                case ProxyType.AutoConfigUrl:
                    _profile.Proxy = new()
                    {
                        AutoConfigUrl = Value
                    };
                    break;
                case ProxyType.Manual:
                    _profile.Proxy = new()
                    {
                        Manual = new()
                        {
                            Server = Value
                        }
                    };
                    break;
            }
            return this;
        }

        public Builder SetDeviceCompliance(bool Enable, string? Eku, string? IssuerHash)
        {
            if (!Enable)
            {
                _profile.DeviceCompliance = null;
            }
            else
            {
                _profile.DeviceCompliance = new()
                {
                    Sso = new()
                    {
                        Eku = Eku,
                        IssuerHash = IssuerHash
                    }
                };
            }
            return this;
        }

        public Builder AddAppTrigger(string AppId)
        {
            _profile.AppTrigger ??= new();
            _profile.AppTrigger.Add(new()
            {
                App = new()
                {
                    Id = AppId
                }
            });
            return this;
        }

        public Builder ResetAppTrigger()
        {
            _profile.AppTrigger = null;
            return this;
        }

        public Builder AddDomainNameInformation(string DomainName, string? DnsServers, string? WebProxyServers, bool? AutoTrigger, bool? Persistent)
        {
            _profile.DomainNameInformation ??= new();
            _profile.DomainNameInformation.Add(new()
            {
                DomainName = DomainName,
                DnsServers = DnsServers,
                WebProxyServers = WebProxyServers,
                AutoTrigger = AutoTrigger,
                Persistent = Persistent
            });
            return this;
        }

        public Builder ResetDomainNameInformation()
        {
            _profile.DomainNameInformation = null;
            return this;
        }

        public Builder AddTrafficFilter(string? AppId, string? Claims, string? Protocol, string? LocalPortRanges, string? RemotePortRanges, string? LocalAddressRanges, string? RemoteAddressRanges, RoutingPolicyType? RoutingPolicyType, string? Direction)
        {
            _profile.TrafficFilter ??= new();
            var item = new TrafficFilter()
            {
                Claims = Claims,
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
                item.App = new()
                {
                    Id = AppId
                };
            }
            _profile.TrafficFilter.Add(item);
            return this;
        }

        public Builder ResetTrafficFilter()
        {
            _profile.TrafficFilter = null;
            return this;
        }

        public Builder SetServers(string Value)
        {
            _profile.NativeProfile ??= new();
            _profile.NativeProfile.Servers = Value;
            return this;
        }

        public Builder SetRoutingPolicyType(RoutingPolicyType? Type)
        {
            _profile.NativeProfile ??= new();
            _profile.NativeProfile.RoutingPolicyType = Type?.ToString();
            return this;
        }

        public Builder SetNativeProtocolType(NativeProtocolType? Type)
        {
            _profile.NativeProfile ??= new();
            _profile.NativeProfile.NativeProtocolType = Type?.ToString();
            return this;
        }

        public Builder SetL2tpPsk(string? Value)
        {
            _profile.NativeProfile ??= new();
            _profile.NativeProfile.L2tpPsk = Value;
            return this;
        }

        public Builder SetDisableClassBasedDefaultRoute(bool? Value)
        {
            _profile.NativeProfile ??= new();
            _profile.NativeProfile.DisableClassBasedDefaultRoute = Value;
            return this;
        }

        public Builder SetPlumbIKEv2TSAsRoutes(bool? Value)
        {
            _profile.NativeProfile ??= new();
            _profile.NativeProfile.PlumbIKEv2TSAsRoutes = Value;
            return this;
        }

        public Builder SetCryptographySuite(
            AuthenticationTransformConstants? authenticationTransformConstants,
            CipherTransformConstants? cipherTransformConstants,
            PfsGroup? pfsGroup,
            DHGroup? dhGroup,
            IntegrityCheckMethod? integrityCheckMethod,
            EncryptionMethod? encryptionMethod)
        {
            _profile.NativeProfile ??= new();
            _profile.NativeProfile.CryptographySuite = new()
            {
                AuthenticationTransformConstants = authenticationTransformConstants?.ToString(),
                CipherTransformConstants = cipherTransformConstants?.ToString(),
                PfsGroup = pfsGroup?.ToString(),
                DHGroup = dhGroup?.ToString(),
                IntegrityCheckMethod = integrityCheckMethod?.ToString(),
                EncryptionMethod = encryptionMethod?.ToString()
            };
            return this;
        }

        public Builder SetAuthentication(
            AuthenticationMethod AuthMethod,
            string? RadiusServerNames,
            List<string>? RadiusServerRootCA,
            bool? DisableServerValidationPrompt,
            List<string>? CertSelectionCA,
            bool? AllPurposeEnabled,
            List<KeyValuePair<string, string>>? CertSelectionEku)
        {
            _profile.NativeProfile ??= new();
            if (AuthMethod == AuthenticationMethod.MachineCert)
            {
                _profile.NativeProfile.Authentication = new()
                {
                    UserMethod = null,
                    Eap = null,
                    MachineMethod = "Certificate"
                };
            }
            else if (AuthMethod == AuthenticationMethod.UserMschapv2)
            {
                _profile.NativeProfile.Authentication = new()
                {
                    UserMethod = "MSChapv2",
                    Eap = null,
                    MachineMethod = null
                };
            }
            else
            {
                var eapBuilder = new EapBuilder(AuthMethod)
                    .SetRadiusServerRootCA(RadiusServerRootCA)
                    .SetDisableServerValidationPrompt(DisableServerValidationPrompt)
                    .SetRadiusServerNames(RadiusServerNames);

                if (CertSelectionCA != null)
                {
                    eapBuilder.AddCertificateSelectionHash(CertSelectionCA);
                }
                if (AllPurposeEnabled != null)
                {
                    eapBuilder.SetCertificateSelectionAllPurpose(AllPurposeEnabled);
                }
                if (CertSelectionEku != null)
                {
                    eapBuilder.AddCertificateSelectionEku(CertSelectionEku);
                }

                var eap = new Eap()
                {
                    XmlConfiguration = eapBuilder.GetXml()
                };

                _profile.NativeProfile.Authentication = new()
                {
                    UserMethod = "EAP",
                    Eap = eap,
                    MachineMethod = null
                };
            }
            return this;
        }

        public Builder AddRoute(string Address, byte Prefix, bool? ExclusionRoute, uint? Metric)
        {
            _profile.Route ??= new();
            _profile.Route.Add(new()
            {
                Address = Address,
                Prefix = Prefix,
                ExclusionRoute = ExclusionRoute,
                Metric = Metric
            });
            return this;
        }

        public string GetXml()
        {
            XmlSerializer serializer = new(_profile.GetType());
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            using var sw = new StringWriter();
            using var xw = XmlWriter.Create(sw, new()
            {
                Encoding = Encoding.UTF8,
                Indent = true,
                OmitXmlDeclaration = true
            });
            serializer.Serialize(xw, _profile, ns);
            var result = Regex.Replace(sw.ToString(), @"\s+<\w+ p\d+:nil=""true"" xmlns:p\d+=""http://www.w3.org/2001/XMLSchema-instance"" />", string.Empty);
            return result;
        }
    }
}