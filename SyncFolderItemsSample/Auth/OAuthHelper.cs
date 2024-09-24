/*
 * By David Barrett, Microsoft Ltd. 2018. Use at your own risk.  No warranties are given.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 * */

using System;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Identity.Client;
using System.Linq;

namespace SyncFolderItemsSample.Auth
{
    public class OAuthHelper
    {
        private static Exception _lastError = null;
        private static string sResourceUrl = "https://outlook.office.com/";
        private static object _oAuthApplication = null;

        public static Exception LastError
        {
            get { return _lastError; }
        }

        public static void RebuildApp()
        {
            _oAuthApplication = null;
        }

        public static string ResourceUrl
        {
            get { return sResourceUrl; }
            set { 
                sResourceUrl = value;
                if (!sResourceUrl.EndsWith("/"))
                    sResourceUrl = $"{sResourceUrl}/";
                }
        }

        public static async Task<AuthenticationResult> GetDelegateToken(string ClientId, string TenantId, string Scope = "EWS.AccessAsUser.All")
        {
            var pcaOptions = new PublicClientApplicationOptions
            {
                ClientId = ClientId,
                TenantId = TenantId
            };
            var ewsScopes = new string[] { $"{sResourceUrl}{Scope}" };


            if (_oAuthApplication != null && _oAuthApplication is PublicClientApplication)
            {
                // This is a token renewal - we try to renew silently first
                try
                {
                    // Make the silent token renewal request
                    var accounts = await ((PublicClientApplication)_oAuthApplication).GetAccountsAsync();
                    AuthenticationResult authResult = await ((PublicClientApplication)_oAuthApplication).AcquireTokenSilent(ewsScopes, accounts.FirstOrDefault()).ExecuteAsync();
                    return authResult;
                }
                catch (Exception ex)
                {
                    _lastError = ex;
                }
            }
            else
            {
                var appBuilder = PublicClientApplicationBuilder
                    .CreateWithApplicationOptions(pcaOptions);

                if (ClientId.Equals("00d8c1e0-fe3c-40d3-8791-0f1132fed50b"))
                    _oAuthApplication = appBuilder.WithRedirectUri("http://localhost/SOAPe");

                _oAuthApplication = appBuilder.Build();
            }

            try
            {
                // Make the interactive token request
                AuthenticationResult authResult = await ((PublicClientApplication)_oAuthApplication).AcquireTokenInteractive(ewsScopes).ExecuteAsync();
                return authResult;
            }
            catch (Exception ex)
            {
                _lastError = ex;
            }
            return null;
        }

        public static async Task<AuthenticationResult> GetApplicationToken(string ClientId, string TenantId, string ClientSecret)
        {
            // Configure the MSAL client to get tokens
            var ewsScopes = new string[] { $"{sResourceUrl}.default" };

            // ConfidentialClientApplication handles it's own cache/renewal
            if (_oAuthApplication == null || !(_oAuthApplication is ConfidentialClientApplication))
                _oAuthApplication = ConfidentialClientApplicationBuilder.Create(ClientId)
                    .WithAuthority(AzureCloudInstance.AzurePublic, TenantId)
                    .WithClientSecret(ClientSecret)
                    .Build();

            AuthenticationResult result = null;
            try
            {
                // Make the token request (should not be interactive, unless Consent required)
                result = await ((ConfidentialClientApplication)_oAuthApplication).AcquireTokenForClient(ewsScopes)
                    .ExecuteAsync();
            }
            catch (Exception ex)
            {
                _lastError = ex;
            }
            return result;
        }

        public static async Task<AuthenticationResult> GetApplicationToken(string ClientId, string TenantId, X509Certificate2 ClientCertificate)
        {
            // Configure the MSAL client to get tokens
            var ewsScopes = new string[] { $"{sResourceUrl}.default" };

            // ConfidentialClientApplication handles it's own cache/renewal
            if (_oAuthApplication == null || !(_oAuthApplication is ConfidentialClientApplication))
                _oAuthApplication = ConfidentialClientApplicationBuilder.Create(ClientId)
                    .WithAuthority(AzureCloudInstance.AzurePublic, TenantId)
                    .WithCertificate(ClientCertificate)
                    .Build();

            AuthenticationResult result = null;
            try
            {
                // Make the token request (should not be interactive, unless Consent required)
                result = await ((ConfidentialClientApplication)_oAuthApplication).AcquireTokenForClient(ewsScopes)
                    .ExecuteAsync();
            }
            catch (Exception ex)
            {
                _lastError = ex;
            }
            return result;
        }
    }
}
