using ProfileXMLBuilder.Lib;

var builder = new Builder()
    .SetDnsSuffix("matrix.net")
    .SetTrustedNetworkDetection("matrix.net")
    .SetProxy(ProxyType.Manual, "10.1.1.1:1234")
    .SetDeviceCompliance(true, null, "00 ab cd ef 00 ab cd ef 00 ab cd ef 00 ab cd ef 00 ab cd ef ")
    .AddAppTrigger("%WINDIR%\\system32\\ping.exe")
    .AddAppTrigger("%WINDIR%\\system32\\tracert.exe")
    .AddTrafficFilter(null, null, "6", null, "1-1024", null, null, RoutingPolicyType.ForceTunnel, "Outbound")
    .AddDomainNameInformation(".contoso.com", "10.1.1.1", null, null, null)
    .AddDomainNameInformation("contoso.com", "10.1.1.1", null, null, null)
    .SetAuthentication(AuthenticationMethod.UserPeapTls,
        "nps.matrix.net",
        new() { "00 ab cd ef 00 ab cd ef 00 ab cd ef 00 ab cd ef 00 ab cd ef ", 
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
    .AddRoute("10.1.1.0", 24, null, null);
Console.WriteLine(builder.GetXml());
