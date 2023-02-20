# ProfileXMLBuilder.PS

This is a simple PowerShell wrapper for the ProfileXMLBuilder library.

It provides a few sample VPN profile settings for quick use.

## Installation

``` PowerShell
Install-Module ProfileXMLBuilder.PS
```

## Usage

### PowerShell Style

You may create all the needed params with PowerShell cmdlets and then pass them to `New-ProfileXMLBuilder`.

To get the final XML, pass the Builder object to `Get-ProfileXML`.

The following is an EAP-TLS L2TP User Split Tunnel example.

``` PowerShell
$rt0 = New-ProfileXMLRoute -Address 10.1.1.128 -Prefix 26 -Metric 30
$rt1 = New-ProfileXMLRoute -Address 10.50.10.128 -Prefix 26 -Metric 30
$tf0 = New-ProfileXMLTrafficFilter -Protocol 6 -RemotePortRanges '80, 443'
$tf1 = New-ProfileXMLTrafficFilter -Protocol 17 -RemotePortRanges '443'
$dni0 = New-ProfileXMLDomainNameInformation -DomainName '.'
$dni1 = New-ProfileXMLDomainNameInformation -DomainName 'fabrikam.com' -DnsServers 10.1.1.1
$dni2 = New-ProfileXMLDomainNameInformation -DomainName '.fabrikam.com' -DnsServers 10.1.1.1 -AutoTrigger -Persistent
$app0 = '%SystemRoot%/System32/ping.exe'
$app1 = '%ProgramFiles%/Vendor/FancyTool.exe'
$auth = New-ProfileXMLAuthentication -UserAuth -UserMethod UserEapTls `
									 -RadiusServerNames 'nps.fabrikam.com' -RadiusServerRootCA '425559414F4855494441425559414F4855494441' `
									 -CertSelectionEku @{ EKU0='OID0'; EKU1='OID1' }

$builder = New-ProfileXMLBuilder -Servers vpn.fabrikam.com `
                                 -DnsSuffix fabrikam.com `
                                 -TrustedNetworkDetection fabrikam.com `
                                 -NativeProtocol Automatic `
                                 -RoutingPolicy SplitTunnel `
                                 -Authentication $auth `
                                 -DomainNameInformation $dni0, $dni1, $dni2 `
                                 -AppTriggers $app0, $app1 `
                                 -ProxyType AutoConfigUrl -ProxyValue 'https://corpprx.fabrikam.com/vpn.js' `
                                 -Routes $rt0,$rt1 `
                                 -TrafficFilters $tf0, $tf1
$builder | Get-ProfileXML
pause
```

### C# Style

First create a Builder instance with `New-ProfileXMLBuilder` cmdlet.

Then set settings needed with methods of the Builder instance. All methods are fluent API.

To check the methods and their parameters, you can call `$builder | GM | select Name, Definition | FT -AutoSize` or check the [C# source code](https://github.com/PixelFrame/ProfileXMLBuilder/blob/master/ProfileXMLBuilder.Lib/Builder.cs).

The following is a PEAP-MSCHAPv2 IKEv2 User Split Tunnel example.

``` PowerShell
$builder = New-ProfileXMLBuilder

$builder.SetServers('vpn.mydomain.net')
$builder.SetDnsSuffix('mydomain.net')
$builder.SetTrustedNetworkDetection('mydomain.net')
$builder.SetNativeProtocolType('IKEv2')
$builder.SetRoutingPolicyType('SplitTunnel')
$builder.AddRoute('10.1.1.0', 24, $null, $null)
$builder.AddRoute('10.1.2.0', 24, $null, $null)
$builder.AddDomainNameInformation('.mydomain.net', '10.1.1.1', $null, $null, $null)
$builder.SetAuthentication('UserPeapMschapv2', 'radius.mydomain.net', '00 11 22 33 44 55 66 77 88 99 aa bb cc dd ee ff 02 05 00 01 ', $true, $null, $null, $null)

$builder.GetXml()
```

Output:
``` XML
<VPNProfile>
  <RememberCredentials>true</RememberCredentials>
  <DnsSuffix>mydomain.net</DnsSuffix>
  <RegisterDNS>true</RegisterDNS>
  <TrustedNetworkDetection>mydomain.net</TrustedNetworkDetection>
  <AlwaysOn>true</AlwaysOn>
  <DeviceTunnel>false</DeviceTunnel>
  <DomainNameInformation>
    <DomainName>.mydomain.net</DomainName>
    <DnsServers>10.1.1.1</DnsServers>
  </DomainNameInformation>
  <NativeProfile>
    <Servers>vpn.mydomain.net</Servers>
    <RoutingPolicyType>SplitTunnel</RoutingPolicyType>
    <NativeProtocolType>IKEv2</NativeProtocolType>
    <DisableClassBasedDefaultRoute>true</DisableClassBasedDefaultRoute>
    <Authentication>
      <UserMethod>EAP</UserMethod>
      <Eap>
        <Configuration>
          <EapHostConfig xmlns="http://www.microsoft.com/provisioning/EapHostConfig">
            <EapMethod>
              <Type xmlns="http://www.microsoft.com/provisioning/EapCommon">25</Type>
              <VendorId xmlns="http://www.microsoft.com/provisioning/EapCommon">0</VendorId>
              <VendorType xmlns="http://www.microsoft.com/provisioning/EapCommon">0</VendorType>
              <AuthorId xmlns="http://www.microsoft.com/provisioning/EapCommon">0</AuthorId>
            </EapMethod>
            <Config xmlns="http://www.microsoft.com/provisioning/EapHostConfig">
              <Eap xmlns="http://www.microsoft.com/provisioning/BaseEapConnectionPropertiesV1">
                <Type>25</Type>
                <EapType xmlns="http://www.microsoft.com/provisioning/MsPeapConnectionPropertiesV1">
                  <ServerValidation>
                    <DisableUserPromptForServerValidation>true</DisableUserPromptForServerValidation>
                    <ServerNames>radius.mydomain.net</ServerNames>
                    <TrustedRootCA>00 11 22 33 44 55 66 77 88 99 aa bb cc dd ee ff 02 05 00 01 </TrustedRootCA>
                  </ServerValidation>
                  <FastReconnect>true</FastReconnect>
                  <InnerEapOptional>false</InnerEapOptional>
                  <Eap xmlns="http://www.microsoft.com/provisioning/BaseEapConnectionPropertiesV1">
                    <Type>26</Type>
                    <EapType xmlns="http://www.microsoft.com/provisioning/MsChapV2ConnectionPropertiesV1">
                      <UseWinLogonCredentials>true</UseWinLogonCredentials>
                    </EapType>
                  </Eap>
                  <EnableQuarantineChecks>false</EnableQuarantineChecks>
                  <RequireCryptoBinding>false</RequireCryptoBinding>
                  <PeapExtensions>
                    <PerformServerValidation xmlns="http://www.microsoft.com/provisioning/MsPeapConnectionPropertiesV2">true</PerformServerValidation>
                    <AcceptServerName xmlns="http://www.microsoft.com/provisioning/MsPeapConnectionPropertiesV2">true</AcceptServerName>
                  </PeapExtensions>
                </EapType>
              </Eap>
            </Config>
          </EapHostConfig>
        </Configuration>
      </Eap>
    </Authentication>
  </NativeProfile>
  <Route>
    <Address>10.1.1.0</Address>
    <Prefix>24</Prefix>
  </Route>
  <Route>
    <Address>10.1.2.0</Address>
    <Prefix>24</Prefix>
  </Route>
</VPNProfile>
```

### Sample Builders

The module packs 5 common VPN configurations.

```
New-SampleDTBuilder
New-SampleUTEapMschapv2Builder
New-SampleUTEapTlsBuilder
New-SampleUTPeapMschapv2Builder
New-SampleUTPeapTlsBuilder
```

If you would like to create a PEAP-TLS IKEv2 User Force Tunnel profile, you can 

``` PowerShell
$builder = New-SampleUTPeapTlsBuilder

$builder.SetServers('vpn.mydomain.net')
$builder.SetDnsSuffix('mydomain.net')
$builder.SetTrustedNetworkDetection('mydomain.net')
$builder.SetNativeProtocolType('IKEv2')
$builder.SetRoutingPolicyType('ForceTunnel')
$builder.ResetDomainNameInformation()
$builder.ResetRoute()
$builder.AddDomainNameInformation('.mydomain.net', "10.1.1.1", $null, $null, $null)
$builder.SetRadiusServerNames('radius0.mydomain.net;radius1.mydomain.net')
$builder.SetRadiusServerRootCA('ab cd ef fe dc ba 01 23 45 67 89 98 76 54 32 10 ff ff ff ff')
$builder.SetCertificateSelectionRootCA('ab cd ef fe dc ba 01 23 45 67 89 98 76 54 32 10 ff ff ff ff')

$builder.GetXml()
```

Output

``` XML
<VPNProfile>
  <RememberCredentials>true</RememberCredentials>
  <DnsSuffix>mydomain.net</DnsSuffix>
  <RegisterDNS>true</RegisterDNS>
  <TrustedNetworkDetection>mydomain.net</TrustedNetworkDetection>
  <AlwaysOn>true</AlwaysOn>
  <DeviceTunnel>false</DeviceTunnel>
  <DomainNameInformation>
    <DomainName>.mydomain.net</DomainName>
    <DnsServers>10.1.1.1</DnsServers>
  </DomainNameInformation>
  <NativeProfile>
    <Servers>vpn.mydomain.net</Servers>
    <RoutingPolicyType>ForceTunnel</RoutingPolicyType>
    <NativeProtocolType>IKEv2</NativeProtocolType>
    <DisableClassBasedDefaultRoute>true</DisableClassBasedDefaultRoute>
    <Authentication>
      <UserMethod>EAP</UserMethod>
      <Eap>
        <Configuration>
          <EapHostConfig xmlns="http://www.microsoft.com/provisioning/EapHostConfig">
            <EapMethod>
              <Type xmlns="http://www.microsoft.com/provisioning/EapCommon">25</Type>
              <VendorId xmlns="http://www.microsoft.com/provisioning/EapCommon">0</VendorId>
              <VendorType xmlns="http://www.microsoft.com/provisioning/EapCommon">0</VendorType>
              <AuthorId xmlns="http://www.microsoft.com/provisioning/EapCommon">0</AuthorId>
            </EapMethod>
            <Config xmlns="http://www.microsoft.com/provisioning/EapHostConfig">
              <Eap xmlns="http://www.microsoft.com/provisioning/BaseEapConnectionPropertiesV1">
                <Type>25</Type>
                <EapType xmlns="http://www.microsoft.com/provisioning/MsPeapConnectionPropertiesV1">
                  <ServerValidation>
                    <DisableUserPromptForServerValidation>false</DisableUserPromptForServerValidation>
                    <ServerNames>radius0.mydomain.net;radius1.mydomain.net</ServerNames>
                    <TrustedRootCA>ab cd ef fe dc ba 01 23 45 67 89 98 76 54 32 10 ff ff ff ff</TrustedRootCA>
                  </ServerValidation>
                  <FastReconnect>true</FastReconnect>
                  <InnerEapOptional>false</InnerEapOptional>
                  <Eap xmlns="http://www.microsoft.com/provisioning/BaseEapConnectionPropertiesV1">
                    <Type>13</Type>
                    <EapType xmlns="http://www.microsoft.com/provisioning/EapTlsConnectionPropertiesV1">
                      <CredentialsSource>
                        <CertificateStore>
                          <SimpleCertSelection>true</SimpleCertSelection>
                        </CertificateStore>
                      </CredentialsSource>
                      <ServerValidation>
                        <DisableUserPromptForServerValidation>false</DisableUserPromptForServerValidation>
                        <ServerNames>radius0.mydomain.net;radius1.mydomain.net</ServerNames>
                        <TrustedRootCA>ab cd ef fe dc ba 01 23 45 67 89 98 76 54 32 10 ff ff ff ff</TrustedRootCA>
                      </ServerValidation>
                      <DifferentUsername>false</DifferentUsername>
                      <PerformServerValidation xmlns="http://www.microsoft.com/provisioning/EapTlsConnectionPropertiesV2">true</PerformServerValidation>
                      <AcceptServerName xmlns="http://www.microsoft.com/provisioning/EapTlsConnectionPropertiesV2">true</AcceptServerName>
                      <TLSExtensions xmlns="http://www.microsoft.com/provisioning/EapTlsConnectionPropertiesV2">
                        <FilteringInfo xmlns="http://www.microsoft.com/provisioning/EapTlsConnectionPropertiesV3">
                          <AllPurposeEnabled>true</AllPurposeEnabled>
                          <CAHashList Enabled="true">
                            <IssuerHash>ab cd ef fe dc ba 01 23 45 67 89 98 76 54 32 10 ff ff ff ff</IssuerHash>
                          </CAHashList>
                        </FilteringInfo>
                      </TLSExtensions>
                    </EapType>
                  </Eap>
                  <EnableQuarantineChecks>false</EnableQuarantineChecks>
                  <RequireCryptoBinding>false</RequireCryptoBinding>
                  <PeapExtensions>
                    <PerformServerValidation xmlns="http://www.microsoft.com/provisioning/MsPeapConnectionPropertiesV2">true</PerformServerValidation>
                    <AcceptServerName xmlns="http://www.microsoft.com/provisioning/MsPeapConnectionPropertiesV2">true</AcceptServerName>
                  </PeapExtensions>
                </EapType>
              </Eap>
            </Config>
          </EapHostConfig>
        </Configuration>
      </Eap>
    </Authentication>
  </NativeProfile>
</VPNProfile>
```