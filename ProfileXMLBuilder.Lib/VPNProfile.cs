using System.Xml.Schema;
using System.Xml;
using System.Xml.Serialization;
using System.Net;

namespace ProfileXMLBuilder.Lib
{
    public class VPNProfile
    {
        public bool? RememberCredentials { get; set; } = true;
        public string? DnsSuffix { get; set; } = "contoso.com";
        public bool? RegisterDNS { get; set; } = true;
        public string? TrustedNetworkDetection { get; set; } = "contoso.com";
        public bool? AlwaysOn { get; set; } = true;
        public bool? DeviceTunnel { get; set; } = false;
        public Proxy? Proxy { get; set; } = null;
        public DeviceCompliance? DeviceCompliance { get; set; } = null;
        [XmlElement("AppTrigger")]
        public List<AppTrigger>? AppTrigger { get; set; } = null;
        [XmlElement("DomainNameInformation")]
        public List<DomainNameInformation>? DomainNameInformation { get; set; } = null;
        [XmlElement("TrafficFilter")]
        public List<TrafficFilter>? TrafficFilter { get; set; } = null;
        public NativeProfile? NativeProfile { get; set; } = new();
        [XmlElement("Route")]
        public List<Route>? Route { get; set; } = null;
    }

    public class Proxy
    {
        public string? AutoConfigUrl { get; set; } = null;
        public ManualSetting? Manual { get; set; } = null;

        public class ManualSetting
        {
            public string Server { get; set; } = string.Empty;
        }
    }

    public class DeviceCompliance
    {
        public bool Enabled { get; set; } = true;
        public Sso? Sso { get; set; } = null;
    }

    public class Sso
    {
        public bool Enabled { get; set; } = true;
        public string? Eku { get; set; } = null;
        private string? issuerHash = null;
        public string? IssuerHash
        {
            get => issuerHash;
            set
            {
                if (value == null)
                {
                    issuerHash = null;
                    return;
                }
                var hash = Helper.CheckAndFormatCertificateHash(value);
                if (hash == string.Empty)
                {
                    throw new InvalidDataException($"Invalid hash string {value}");
                }
                issuerHash = hash;
            }
        }
    }

    public class AppTrigger
    {
        public App App { get; set; } = new();
    }

    public class App
    {
        public string Id = string.Empty;
    }

    public class DomainNameInformation
    {
        public string DomainName { get; set; } = string.Empty;
        public string? DnsServers { get; set; } = null;
        public string? WebProxyServers { get; set; } = null;
        public bool? AutoTrigger { get; set; } = null;
        public bool? Persistent { get; set; } = null;
    }

    public class TrafficFilter
    {
        public App? App { get; set; } = null;
        public string? Claims { get; set; } = null;
        private string? protocol = null;
        public string? Protocol
        {
            get => protocol;
            set
            {
                if (byte.TryParse(value, out _))
                {
                    protocol = value;
                }
                else
                {
                    throw new InvalidDataException("Protocol should be a numeric value in 0-255");
                }
            }
        }
        private string? localPortRanges = null;
        public string? LocalPortRanges
        {
            get => localPortRanges;
            set
            {
                if (!Helper.CheckPortRangeValue(value, out var f))
                {
                    throw new InvalidDataException($"${f} is not a valid port range");
                }
                else localPortRanges = value;
            }
        }
        private string? remotePortRanges = null;
        public string? RemotePortRanges
        {
            get => remotePortRanges;
            set
            {
                if (!Helper.CheckPortRangeValue(value, out var f))
                {
                    throw new InvalidDataException($"\"{f}\" is not a valid port range");
                }
                else remotePortRanges = value;
            }
        }
        private string? localAddressRanges = null;
        public string? LocalAddressRanges
        {
            get => localAddressRanges;
            set
            {
                if (!Helper.CheckAddressRangeValue(value, out var f))
                {
                    throw new InvalidDataException($"\"{f}\" is not a valid address range");
                }
                else localAddressRanges = value;
            }
        }
        private string? remoteAddressRanges = null;
        public string? RemoteAddressRanges
        {
            get => remoteAddressRanges;
            set
            {
                if (!Helper.CheckAddressRangeValue(value, out var f))
                {
                    throw new InvalidDataException($"\"{f}\" is not a valid address range");
                }
                else remoteAddressRanges = value;
            }
        }
        public string? RoutingPolicyType { get; set; } = null;
        public string? Direction { get; set; } = null;
    }

    public class NativeProfile
    {
        public string Servers { get; set; } = "vpn.contoso.com";
        public string? RoutingPolicyType { get; set; } = "SplitTunnel";
        public string? NativeProtocolType { get; set; } = "Automatic";
        public string? L2tpPsk { get; set; } = null;
        public bool? DisableClassBasedDefaultRoute { get; set; } = true;
        public bool? PlumbIKEv2TSAsRoutes { get; set; } = null;
        public CryptographySuite? CryptographySuite { get; set; } = null;
        public Authentication Authentication { get; set; } = new();
    }

    public class CryptographySuite
    {
        public string? AuthenticationTransformConstants { get; set; } = null;
        public string? CipherTransformConstants { get; set; } = null;
        public string? PfsGroup { get; set; } = null;
        public string? DHGroup { get; set; } = null;
        public string? IntegrityCheckMethod { get; set; } = null;
        public string? EncryptionMethod { get; set; } = null;
    }

    public class Authentication
    {
        public string? UserMethod { get; set; } = "EAP";
        public Eap? Eap { get; set; } = null;
        public string? MachineMethod { get; set; } = null;
    }

    public class Route
    {
        private string address = "0.0.0.0";
        public string Address
        {
            get => address;
            set
            {
                if (IPAddress.TryParse(value, out _))
                {
                    address = value;
                }
                else
                {
                    throw new InvalidDataException($"\"{value}\" is not a valid address");
                }
            }
        }
        public byte Prefix { get; set; } = 0;
        public bool? ExclusionRoute { get; set; } = null;
        public uint? Metric { get; set; } = null;
    }

    public class Eap : IXmlSerializable
    {
        [XmlElement("Configuration")]
        public string XmlConfiguration { get; set; } = string.Empty;

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("Configuration");
            writer.WriteRaw(XmlConfiguration);
            writer.WriteEndElement();
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadToDescendant("Configuration");
            XmlConfiguration = reader.ReadInnerXml();
            reader.ReadEndElement();
        }

        public XmlSchema? GetSchema()
        {
            return null;
        }
    }
}
