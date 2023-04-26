using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ProfileXMLBuilder.Lib
{
    public class EapBuilder
    {
        private const string EapTlsTemplate =
@"          <EapHostConfig xmlns=""http://www.microsoft.com/provisioning/EapHostConfig"">
            <EapMethod>
              <Type xmlns=""http://www.microsoft.com/provisioning/EapCommon"">13</Type>
              <VendorId xmlns=""http://www.microsoft.com/provisioning/EapCommon"">0</VendorId>
              <VendorType xmlns=""http://www.microsoft.com/provisioning/EapCommon"">0</VendorType>
              <AuthorId xmlns=""http://www.microsoft.com/provisioning/EapCommon"">0</AuthorId>
            </EapMethod>
            <Config>
              <Eap xmlns=""http://www.microsoft.com/provisioning/BaseEapConnectionPropertiesV1"">
                <Type>13</Type>
                <EapType xmlns=""http://www.microsoft.com/provisioning/EapTlsConnectionPropertiesV1"">
                  <CredentialsSource>
                    <CertificateStore>
                      <SimpleCertSelection>true</SimpleCertSelection>
                    </CertificateStore>
                  </CredentialsSource>
                  <ServerValidation>
                    <DisableUserPromptForServerValidation>{DisableServerValidationPrompt}</DisableUserPromptForServerValidation>
                    <ServerNames>{RadiusServerNames}</ServerNames>
{EapTrustedRootCA}
                  </ServerValidation>
                  <DifferentUsername>false</DifferentUsername>
                  <PerformServerValidation xmlns=""http://www.microsoft.com/provisioning/EapTlsConnectionPropertiesV2"">true</PerformServerValidation>
                  <AcceptServerName xmlns=""http://www.microsoft.com/provisioning/EapTlsConnectionPropertiesV2"">true</AcceptServerName>
{TlsExtension}
                </EapType>
              </Eap>
            </Config>
          </EapHostConfig>
        ";

        private const string EapMschapv2Template =
@"          <EapHostConfig xmlns=""http://www.microsoft.com/provisioning/EapHostConfig"">
            <EapMethod>
              <Type xmlns=""http://www.microsoft.com/provisioning/EapCommon"">26</Type>
              <VendorId xmlns=""http://www.microsoft.com/provisioning/EapCommon"">0</VendorId>
              <VendorType xmlns=""http://www.microsoft.com/provisioning/EapCommon"">0</VendorType>
              <AuthorId xmlns=""http://www.microsoft.com/provisioning/EapCommon"">0</AuthorId>
            </EapMethod>
            <Config xmlns=""http://www.microsoft.com/provisioning/EapHostConfig"">
              <Eap xmlns=""http://www.microsoft.com/provisioning/BaseEapConnectionPropertiesV1"">
                <Type>26</Type>
                <EapType xmlns=""http://www.microsoft.com/provisioning/MsChapV2ConnectionPropertiesV1"">
                  <UseWinLogonCredentials>true</UseWinLogonCredentials>
                </EapType>
              </Eap>
            </Config>
          </EapHostConfig>
        ";

        private const string PeapTlsTemplate =
@"          <EapHostConfig xmlns=""http://www.microsoft.com/provisioning/EapHostConfig"">
            <EapMethod>
              <Type xmlns=""http://www.microsoft.com/provisioning/EapCommon"">25</Type>
              <VendorId xmlns=""http://www.microsoft.com/provisioning/EapCommon"">0</VendorId>
              <VendorType xmlns=""http://www.microsoft.com/provisioning/EapCommon"">0</VendorType>
              <AuthorId xmlns=""http://www.microsoft.com/provisioning/EapCommon"">0</AuthorId>
            </EapMethod>
            <Config xmlns=""http://www.microsoft.com/provisioning/EapHostConfig"">
              <Eap xmlns=""http://www.microsoft.com/provisioning/BaseEapConnectionPropertiesV1"">
                <Type>25</Type>
                <EapType xmlns=""http://www.microsoft.com/provisioning/MsPeapConnectionPropertiesV1"">
                  <ServerValidation>
                    <DisableUserPromptForServerValidation>{DisableServerValidationPrompt}</DisableUserPromptForServerValidation>
                    <ServerNames>{RadiusServerNames}</ServerNames>
{PeapTrustedRootCA}
                  </ServerValidation>
                  <FastReconnect>true</FastReconnect>
                  <InnerEapOptional>false</InnerEapOptional>
                  <Eap xmlns=""http://www.microsoft.com/provisioning/BaseEapConnectionPropertiesV1"">
                    <Type>13</Type>
                    <EapType xmlns=""http://www.microsoft.com/provisioning/EapTlsConnectionPropertiesV1"">
                      <CredentialsSource>
                        <CertificateStore>
                          <SimpleCertSelection>true</SimpleCertSelection>
                        </CertificateStore>
                      </CredentialsSource>
                      <ServerValidation>
                        <DisableUserPromptForServerValidation>{DisableServerValidationPrompt}</DisableUserPromptForServerValidation>
                        <ServerNames>{RadiusServerNames}</ServerNames>
{PeapEapTrustedRootCA}
                      </ServerValidation>
                      <DifferentUsername>false</DifferentUsername>
                      <PerformServerValidation xmlns=""http://www.microsoft.com/provisioning/EapTlsConnectionPropertiesV2"">true</PerformServerValidation>
                      <AcceptServerName xmlns=""http://www.microsoft.com/provisioning/EapTlsConnectionPropertiesV2"">true</AcceptServerName>
{TlsExtension}
                    </EapType>
                  </Eap>
                  <EnableQuarantineChecks>false</EnableQuarantineChecks>
                  <RequireCryptoBinding>false</RequireCryptoBinding>
                  <PeapExtensions>
                    <PerformServerValidation xmlns=""http://www.microsoft.com/provisioning/MsPeapConnectionPropertiesV2"">true</PerformServerValidation>
                    <AcceptServerName xmlns=""http://www.microsoft.com/provisioning/MsPeapConnectionPropertiesV2"">true</AcceptServerName>
                  </PeapExtensions>
                </EapType>
              </Eap>
            </Config>
          </EapHostConfig>
        ";

        private const string PeapMschapv2Template =
@"          <EapHostConfig xmlns=""http://www.microsoft.com/provisioning/EapHostConfig"">
            <EapMethod>
              <Type xmlns=""http://www.microsoft.com/provisioning/EapCommon"">25</Type>
              <VendorId xmlns=""http://www.microsoft.com/provisioning/EapCommon"">0</VendorId>
              <VendorType xmlns=""http://www.microsoft.com/provisioning/EapCommon"">0</VendorType>
              <AuthorId xmlns=""http://www.microsoft.com/provisioning/EapCommon"">0</AuthorId>
            </EapMethod>
            <Config xmlns=""http://www.microsoft.com/provisioning/EapHostConfig"">
              <Eap xmlns=""http://www.microsoft.com/provisioning/BaseEapConnectionPropertiesV1"">
                <Type>25</Type>
                <EapType xmlns=""http://www.microsoft.com/provisioning/MsPeapConnectionPropertiesV1"">
                  <ServerValidation>
                    <DisableUserPromptForServerValidation>{DisableServerValidationPrompt}</DisableUserPromptForServerValidation>
                    <ServerNames>{RadiusServerNames}</ServerNames>
{PeapTrustedRootCA}
                  </ServerValidation>
                  <FastReconnect>true</FastReconnect>
                  <InnerEapOptional>false</InnerEapOptional>
                  <Eap xmlns=""http://www.microsoft.com/provisioning/BaseEapConnectionPropertiesV1"">
                    <Type>26</Type>
                    <EapType xmlns=""http://www.microsoft.com/provisioning/MsChapV2ConnectionPropertiesV1"">
                      <UseWinLogonCredentials>true</UseWinLogonCredentials>
                    </EapType>
                  </Eap>
                  <EnableQuarantineChecks>false</EnableQuarantineChecks>
                  <RequireCryptoBinding>false</RequireCryptoBinding>
                  <PeapExtensions>
                    <PerformServerValidation xmlns=""http://www.microsoft.com/provisioning/MsPeapConnectionPropertiesV2"">true</PerformServerValidation>
                    <AcceptServerName xmlns=""http://www.microsoft.com/provisioning/MsPeapConnectionPropertiesV2"">true</AcceptServerName>
                  </PeapExtensions>
                </EapType>
              </Eap>
            </Config>
          </EapHostConfig>
        ";

        private const string CertSelection =
@"<TLSExtensions xmlns=""http://www.microsoft.com/provisioning/EapTlsConnectionPropertiesV2"">
  <FilteringInfo xmlns=""http://www.microsoft.com/provisioning/EapTlsConnectionPropertiesV3"">
    <AllPurposeEnabled>{AllPurposeEnabled}</AllPurposeEnabled>
{CAHashList}
{EKUList}
  </FilteringInfo>
</TLSExtensions>";

        private string _template = string.Empty;
        private AuthenticationMethod _authMethod;

        public EapBuilder(AuthenticationMethod AuthMethod)
        {
            _authMethod = AuthMethod;
            switch (AuthMethod)
            {
                case AuthenticationMethod.UserEapTls: _template = EapTlsTemplate; break;
                case AuthenticationMethod.UserEapMschapv2: _template = EapMschapv2Template; break;
                case AuthenticationMethod.UserPeapTls: _template = PeapTlsTemplate; break;
                case AuthenticationMethod.UserPeapMschapv2: _template += PeapMschapv2Template; break;
                default: throw new InvalidOperationException("Provided authentication method is not a supported EAP method");
            }
        }

        public EapBuilder SetRadiusServerNames(string? Value)
        {
            _template = _template.Replace("{RadiusServerNames}", Value ?? string.Empty);
            return this;
        }

        public EapBuilder SetRadiusServerRootCA(List<string>? Value)
        {
            if (Value == null)
            {
                _template = Regex.Replace(_template, @"^.*?{TrustedRootCA}.*?$", string.Empty, RegexOptions.Multiline);
            }
            else
            {
                var sbEap = new StringBuilder();
                var sbPeapEap = new StringBuilder();
                var eapIntend = "                    ";
                var peapEapIntend = "                        ";
                foreach (var ca in Value)
                {
                    var hash = Helper.CheckAndFormatCertificateHash(ca);
                    if (hash == string.Empty)
                    {
                        throw new InvalidDataException($"Invalid hash string {ca}");
                    }

                    sbEap.AppendLine($"{eapIntend}<TrustedRootCA>{hash}</TrustedRootCA>");
                    sbPeapEap.AppendLine($"{peapEapIntend}<TrustedRootCA>{hash}</TrustedRootCA>");
                }
                _template = _template.Replace("{EapTrustedRootCA}", sbEap.ToString())
                                     .Replace("{PeapTrustedRootCA}", sbEap.ToString())
                                     .Replace("{PeapEapTrustedRootCA}", sbPeapEap.ToString());
            }
            return this;
        }

        public EapBuilder SetDisableServerValidationPrompt(bool? Value)
        {
            _template = _template.Replace("{DisableServerValidationPrompt}", (Value == null || Value == false) ? "false" : "true");
            return this;
        }

        private void ReplaceTlsExtensionIfNotDone()
        {
            if (_template.Contains("{TlsExtension}"))
            {
                var intend = _authMethod == AuthenticationMethod.UserEapTls ? "                  " : "                      ";
                var certSelectionWithIntend = Regex.Replace(CertSelection, @"^(\s*<.*)$", $"{intend}$1", RegexOptions.Multiline);
                _template = _template.Replace("{TlsExtension}", certSelectionWithIntend);
            }
        }

        public EapBuilder SetCertificateSelectionAllPurpose(bool? Value)
        {
            ReplaceTlsExtensionIfNotDone();
            _template = _template.Replace("{AllPurposeEnabled}", (Value == null || Value == true) ? "true" : "false");
            return this;
        }

        public EapBuilder AddCertificateSelectionHash(List<string> Value)
        {
            ReplaceTlsExtensionIfNotDone();
            const string XmlCAHashList =
@"<CAHashList Enabled=""true"">
{IssuerHash}
</CAHashList>";

            var intend = _authMethod == AuthenticationMethod.UserEapTls ? "                      " : "                          ";
            var XmlCAHashListWithIntend = Regex.Replace(XmlCAHashList, @"^(\s*<.*)$", $"{intend}$1", RegexOptions.Multiline);

            _template = _template.Replace("{CAHashList}", XmlCAHashListWithIntend);
            var sb = new StringBuilder();
            foreach (var ca in Value)
            {
                var hash = Helper.CheckAndFormatCertificateHash(ca);
                if (hash == string.Empty)
                {
                    throw new InvalidDataException($"Invalid hash string {ca}");
                }

                sb.AppendLine($"{intend}  <IssuerHash>{hash}</IssuerHash>");
            }
            _template = _template.Replace("{IssuerHash}", sb.ToString());
            return this;
        }

        public EapBuilder AddCertificateSelectionEku(List<Eku> Value)
        {
            ReplaceTlsExtensionIfNotDone();
            const string XmlEkuList =
@"<EKUMapping>
{EKUMap}
</EKUMapping>
<ClientAuthEKUList Enabled=""true"">
  <EKUMapInList>
{EKUName}
  </EKUMapInList>
</ClientAuthEKUList>";

            var intend = _authMethod == AuthenticationMethod.UserEapTls ? "                      " : "                          ";
            var XmlEkuListWithIntend = Regex.Replace(XmlEkuList, @"^(\s*<.*)$", $"{intend}$1", RegexOptions.Multiline);

            _template = _template.Replace("{EKUList}", XmlEkuListWithIntend);
            var sbEkuMap = new StringBuilder();
            var sbEkuName = new StringBuilder();
            foreach (var eku in Value)
            {
                sbEkuMap.AppendLine($"{intend}  <EKUMap>");
                sbEkuMap.AppendLine($"{intend}    <EKUName>{eku.Name}</EKUName>");
                sbEkuMap.AppendLine($"{intend}    <EKUOID>{eku.OID}</EKUOID>");
                sbEkuMap.AppendLine($"{intend}  </EKUMap>");
                sbEkuName.AppendLine($"{intend}    <EKUName>{eku.Name}</EKUName>");
            }
            _template = _template.Replace("{EKUMap}", sbEkuMap.ToString());
            _template = _template.Replace("{EKUName}", sbEkuName.ToString());
            return this;
        }

        public string GetXml()
        {
            _template = Regex.Replace(_template, @"^.*{.*}.*$", string.Empty, RegexOptions.Multiline);
            _template = Regex.Replace(_template, @"^\s*\n", string.Empty, RegexOptions.Multiline);
            return Environment.NewLine + _template;
        }
    }
}
