using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ProfileXMLBuilder.Lib
{
    internal static class Helper
    {
        internal static bool CheckPortRangeValue(string? Value, out string Faulty)
        {
            if (Value == null)
            {
                Faulty = string.Empty;
                return true;
            }
            var ranges = Value.Split(',').Select(r => r.Trim());
            foreach (var range in ranges)
            {
                if (range.Contains('-'))
                {
                    var ports = range.Split('-');
                    if (ports.Length > 2)
                    {
                        Faulty = range;
                        return false;
                    }
                    var begin = ports[0];
                    var end = ports[1];
                    if (int.TryParse(begin, out var b) && int.TryParse(end, out var e))
                    {
                        if (b < 1 || b > 65535 || e < 1 || e > 65535 || b > e)
                        {
                            Faulty = range;
                            return false;
                        }
                    }
                    else
                    {
                        Faulty = range;
                        return false;
                    }
                }
                else
                {
                    if (!int.TryParse(range, out var p))
                    {
                        if (p < 1 || p > 65535)
                        {
                            Faulty = range;
                            return false;
                        }
                    }
                }
            }
            Faulty = string.Empty;
            return true;
        }

        internal static bool CheckAddressRangeValue(string? Value, out string Faulty)
        {
            if (Value == null)
            {
                Faulty = string.Empty;
                return true;
            }
            var addresses = Value.Split(',').Select(a => a.Trim());
            foreach (var address in addresses)
            {
                if (address.Contains('/'))
                {
                    var ip = address.Substring(0, address.IndexOf('/'));
                    var mask = address.Substring(address.IndexOf('/') + 1);
                    if (IPAddress.TryParse(ip, out var ipAddr) && int.TryParse(mask, out var m))
                    {
                        if (ipAddr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            if (m < 1 || m > 32)
                            {
                                Faulty = address;
                                return false;
                            }
                        }
                        else if (ipAddr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                        {
                            if (m < 1 || m > 128)
                            {
                                Faulty = address;
                                return false;
                            }
                        }
                        else
                        {
                            Faulty = address;
                            return false;
                        }
                    }
                    else
                    {
                        Faulty = address;
                        return false;
                    }
                }
                else if (address.Contains('-'))
                {
                    var ips = address.Split('-');
                    if (ips.Length > 2)
                    {
                        Faulty = address;
                        return false;
                    }
                    if (IPAddress.TryParse(ips[0], out var b) && IPAddress.TryParse(ips[1], out var e))
                    {
                        if (
                            b.AddressFamily != e.AddressFamily ||
                           (b.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork &&
                            b.AddressFamily != System.Net.Sockets.AddressFamily.InterNetworkV6)
                           )
                        {
                            Faulty = address;
                            return false;
                        }
                        if (b.IPAddressComparison(e) < 0)
                        {
                            Faulty = address;
                            return false;
                        }

                    }
                    else
                    {
                        Faulty = address;
                        return false;
                    }
                }
                else
                {
                    if (!IPAddress.TryParse(address, out _))
                    {
                        Faulty = address;
                        return false;
                    }
                }
            }

            Faulty = string.Empty;
            return true;
        }

        internal static int IPAddressComparison(this IPAddress addr0, IPAddress add1)
        {
            return ByteArrayComparison(addr0.GetAddressBytes(), add1.GetAddressBytes());
        }

        internal static int ByteArrayComparison(this byte[] a, byte[] b)
        {
            if (a.Length > b.Length) return 1;
            if (a.Length < b.Length) return -1;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == b[i]) continue;
                if (a[i] < b[i]) return -1;
                if (a[i] > b[i]) return -1;
            }
            return 0;
        }

        internal static string CheckAndFormatCertificateHash(string hash)
        {
            hash = hash.Replace(" ", "");
            if (hash.Length % 2 != 0)
            {
                return string.Empty;
            }
            else
            {
                const string hextokens = "0123456789abcdefABCDEF";
                var sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i += 2)
                {
                    if (hextokens.Contains(hash[i]) && hextokens.Contains(hash[i + 1]))
                    {
                        sb.Append(hash[i]);
                        sb.Append(hash[i + 1]);
                        sb.Append(' ');
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                return sb.ToString();
            }
        }
    }
}
