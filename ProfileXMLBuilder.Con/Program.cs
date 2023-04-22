using ProfileXMLBuilder.Lib;

var builder = new Builder()
    .SetDnsSuffix("matrix.net")
    .SetTrustedNetworkDetection("matrix.net")
    .SetProxy(ProxyType.Manual, "10.1.1.1:1234")
    .SetDeviceCompliance(true, null, "00abcdef00abcdef00abcdef00abcdef00abcdef")
    .AddAppTrigger("%WINDIR%\\system32\\ping.exe")
    .AddAppTrigger("%WINDIR%\\system32\\tracert.exe")
    .AddTrafficFilter(null, null, null, null, "21-10245, 1", "10.1.1.1-10.1.1.50", "10.1.1.1, 10.1.1.2", RoutingPolicyType.ForceTunnel, TrafficDirection.Outbound)
    .AddDomainNameInformation(".contoso.com", "10.1.1.1", null, null, null)
    .AddDomainNameInformation("contoso.com", "10.1.1.1", null, null, null)
    .SetAuthentication(AuthenticationMethod.UserPeapTls,
        "nps.matrix.net",
        new() { "00 ab cd ef 00 ab cd ef 00 ab cd e1 00 ab cd ef 00 ab cd ef ",
                "a8 98 5d 3a 65 e5 e5 c4 b2 d7 d6 6d 40 c6 dd 2f b1 9c 54 36 " },
        false,
        new() { "00 ab cd ef 00 ab cd ef 00 ab cd ef 00 ab cd ef 00 ab cd ef ",
                "a8 98 5d 3a 65 e5 e5 c4 b2 d7 d6 6d 40 c6 dd 2f b1 9c 54 36 " },
        true,
        new()
        {
            new("EKU", "OID"),
            new("EKU2", "OID2"),
            new("EKU3", "OID3")
        })
    .AddRoute("2.1.1.255", 24, null, null)
    .SetPrivateNetwork(true)
    .SetUseRasCredentials(false)
    .SetNativeProtocolList(new NativeProtocolListType[] { NativeProtocolListType.IKEv2, NativeProtocolListType.SSTP })
    ;
builder.SetRadiusServerRootCA(new()
{
    "1827501027562080182750102756208018275010275620801827501027562080",
    "425559414F4855494441425559414F4855494441"
});
Console.WriteLine(builder.GetXml());
Console.WriteLine("Is Win11 Profile? " + builder.Win11Profile);
