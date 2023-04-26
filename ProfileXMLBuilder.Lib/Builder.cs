using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace ProfileXMLBuilder.Lib
{
    public class Builder
    {
        private readonly VPNProfile _profile = new();

        private AuthenticationMethod _AuthMethod;
        private string? _RadiusServerNames;
        private List<string>? _RadiusServerRootCA;
        private bool? _DisableServerValidationPrompt;
        private List<string>? _CertSelectionCA;
        private bool? _AllPurposeEnabled;
        private List<Eku>? _CertSelectionEku;

        public bool Win11Profile
        {
            get
            {
                var _ = _profile.DataEncryption == null &&
                    _profile.DisableAdvancedOptionsEditButton == null &&
                    _profile.DisableDisconnectButton == null &&
                    _profile.DisableIKEv2Fragmentation == null &&
                    _profile.IPv4InterfaceMetric == null &&
                    _profile.IPv6InterfaceMetric == null &&
                    _profile.NetworkOutageTime == null &&
                    _profile.PrivateNetwork == null &&
                    _profile.UseRasCredentials == null;
                if (_profile.NativeProfile != null)
                {
                    _ = _ &&
                        _profile.NativeProfile.NativeProtocolType != "ProtocolList" &&
                        _profile.NativeProfile.NativeProtocolType != "SSTP" &&
                        _profile.NativeProfile.ProtocolList == null;
                }
                return !_;
            }
        }

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

        public Builder SetAlwaysOnActive(bool? Value)
        {
            _profile.AlwaysOnActive = Value;
            return this;
        }

        public Builder SetDeviceTunnel(bool? Value)
        {
            _profile.DeviceTunnel = Value;
            return this;
        }

        public Builder SetByPassForLocal(bool? Value)
        {
            _profile.ByPassForLocal = Value;
            return this;
        }

        public Builder SetDataEncryption(DataEncryptionLevel? Value)
        {
            _profile.DataEncryption = Value?.ToString();
            return this;
        }

        public Builder SetDisableAdvancedOptionsEditButton(bool? Value)
        {
            _profile.DisableAdvancedOptionsEditButton = Value;
            return this;
        }

        public Builder SetDisableDisconnectButton(bool? Value)
        {
            _profile.DisableDisconnectButton = Value;
            return this;
        }

        public Builder SetDisableIKEv2Fragmentation(bool? Value)
        {
            _profile.DisableIKEv2Fragmentation = Value;
            return this;
        }

        public Builder SetIPv4InterfaceMetric(int? Value)
        {
            _profile.IPv4InterfaceMetric = Value;
            return this;
        }

        public Builder SetIPv6InterfaceMetric(int? Value)
        {
            _profile.IPv6InterfaceMetric = Value;
            return this;
        }

        public Builder SetNetworkOutageTime(uint? Value)
        {
            _profile.NetworkOutageTime = Value;
            return this;
        }

        public Builder SetPrivateNetwork(bool? Value)
        {
            _profile.PrivateNetwork = Value;
            return this;
        }

        public Builder SetUseRasCredentials(bool? Value)
        {
            _profile.UseRasCredentials = Value;
            return this;
        }
        public Builder SetProxy(Proxy? Value)
        {
            _profile.Proxy = Value;
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

        public Builder AddDomainNameInformation(DomainNameInformation[] domainNameInformation)
        {
            _profile.DomainNameInformation ??= new();
            _profile.DomainNameInformation.AddRange(domainNameInformation);
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

        public Builder AddTrafficFilters(TrafficFilter[] trafficFilters)
        {
            _profile.TrafficFilter ??= new();
            _profile.TrafficFilter.AddRange(trafficFilters);
            return this;
        }

        public Builder AddTrafficFilter(string? AppId, string? Claims, string? Protocol, string? LocalPortRanges, string? RemotePortRanges, string? LocalAddressRanges, string? RemoteAddressRanges, RoutingPolicyType? RoutingPolicyType, TrafficDirection? Direction)
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
                Direction = Direction?.ToString()
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
            if (Type == NativeProtocolType.ProtocolList)
            {
                throw new InvalidOperationException("Call SetNativeProtocolList to set ProtocolList");
            }
            _profile.NativeProfile ??= new();
            _profile.NativeProfile.NativeProtocolType = Type?.ToString();
            _profile.NativeProfile.ProtocolList = null;
            return this;
        }

        public Builder SetNativeProtocolList(NativeProtocolListType[] Types, int? RetryTimeInHours = null)
        {
            _profile.NativeProfile ??= new();
            _profile.NativeProfile.NativeProtocolType = NativeProtocolType.ProtocolList.ToString();
            _profile.NativeProfile.ProtocolList = new();
            foreach (var t in Types)
            {
                _profile.NativeProfile.ProtocolList.NativeProtocol.Add(new() { Type = t.ToString() });
            }
            _profile.NativeProfile.ProtocolList.RetryTimeInHours = RetryTimeInHours;
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

        public Builder SetCryptographySuite(CryptographySuite? Value)
        {
            _profile.NativeProfile ??= new();
            _profile.NativeProfile.CryptographySuite = Value;
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
            List<Eku>? CertSelectionEku)
        {
            // Save the state for the partial modification
            _AuthMethod = AuthMethod;
            _RadiusServerNames = RadiusServerNames;
            _RadiusServerRootCA = RadiusServerRootCA;
            _DisableServerValidationPrompt = DisableServerValidationPrompt;
            _CertSelectionCA = CertSelectionCA;
            _AllPurposeEnabled = AllPurposeEnabled;
            _CertSelectionEku = CertSelectionEku;

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
                var eapBuilder = new EapBuilder(AuthMethod);
                if (AuthMethod != AuthenticationMethod.UserEapMschapv2)
                {
                    eapBuilder.SetRadiusServerRootCA(RadiusServerRootCA)
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

        public Builder SetRadiusServerNames(string Value)
        {
            if (_AuthMethod != AuthenticationMethod.UserEapTls &&
                _AuthMethod != AuthenticationMethod.UserPeapTls &&
                _AuthMethod != AuthenticationMethod.UserPeapMschapv2)
            {
                throw new InvalidOperationException("Current Authentication Method does not need to set RADIUS server names");
            }
            SetAuthentication(_AuthMethod, Value, _RadiusServerRootCA, _DisableServerValidationPrompt, _CertSelectionCA, _AllPurposeEnabled, _CertSelectionEku);
            return this;
        }

        public Builder SetRadiusServerRootCA(List<string> Value)
        {
            if (_AuthMethod != AuthenticationMethod.UserEapTls &&
                _AuthMethod != AuthenticationMethod.UserPeapTls &&
                _AuthMethod != AuthenticationMethod.UserPeapMschapv2)
            {
                throw new InvalidOperationException("Current Authentication Method does not need to set RADIUS root CA");
            }
            SetAuthentication(_AuthMethod, _RadiusServerNames, Value, _DisableServerValidationPrompt, _CertSelectionCA, _AllPurposeEnabled, _CertSelectionEku);
            return this;
        }

        public Builder SetDisableServerValidationPrompt(bool Value)
        {
            if (_AuthMethod != AuthenticationMethod.UserEapTls &&
                _AuthMethod != AuthenticationMethod.UserPeapTls &&
                _AuthMethod != AuthenticationMethod.UserPeapMschapv2)
            {
                throw new InvalidOperationException("Current Authentication Method does not need to set RADIUS server validation");
            }
            SetAuthentication(_AuthMethod, _RadiusServerNames, _RadiusServerRootCA, Value, _CertSelectionCA, _AllPurposeEnabled, _CertSelectionEku);
            return this;
        }

        public Builder SetCertificateSelectionRootCA(List<string>? Value)
        {
            SetAuthentication(_AuthMethod, _RadiusServerNames, _RadiusServerRootCA, _DisableServerValidationPrompt, Value, _AllPurposeEnabled, _CertSelectionEku);
            return this;
        }

        public Builder SetCertificateSelectionEku(List<Eku>? Value)
        {
            SetAuthentication(_AuthMethod, _RadiusServerNames, _RadiusServerRootCA, _DisableServerValidationPrompt, _CertSelectionCA, _AllPurposeEnabled, Value);
            return this;
        }

        public Builder SetCertificateSelectionAllPurposeEnabled(bool? Value)
        {
            SetAuthentication(_AuthMethod, _RadiusServerNames, _RadiusServerRootCA, _DisableServerValidationPrompt, _CertSelectionCA, Value, _CertSelectionEku);
            return this;
        }

        public Builder AddRoutes(Route[] route)
        {
            _profile.Route ??= new();
            _profile.Route.AddRange(route);
            return this;
        }

        public Builder AddRoute(string Address, byte PrefixSize, bool? ExclusionRoute, uint? Metric)
        {
            _profile.Route ??= new();
            _profile.Route.Add(new()
            {
                Address = Address,
                PrefixSize = PrefixSize,
                ExclusionRoute = ExclusionRoute,
                Metric = Metric
            });
            return this;
        }

        public Builder ResetRoute()
        {
            _profile.Route = null;
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
            var result = Regex.Replace(sw.ToString(), @"\s+<.* />", string.Empty); // Remove all XML tags without content, as PowerShell will convert $null to string.Empty leading to empty tags
            return result;
        }
    }
}