/*
 * By David Barrett, Microsoft Ltd. 2020. Use at your own risk.  No warranties are given.
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
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Exchange.WebServices.Autodiscover;

namespace SyncFolderItemsSample.Auth
{
    public enum AuthType
    {
        None,
        Default,
        Basic,
        Certificate,
        OAuth
    }

    public class CredentialHandler
    {
        String _password = String.Empty;
        X509Certificate2 _certificate = null;
        AuthType _authType = AuthType.Basic;
        private FormAzureApplicationRegistration _oAuthAppRegForm = null;

        public CredentialHandler(AuthType authType)
        {
            _authType = authType;
        }

        public CredentialHandler(FormAzureApplicationRegistration appRegForm)
        {
            _oAuthAppRegForm = appRegForm;
            _authType = AuthType.OAuth;
        }


        public string Username { get; set; } = String.Empty;

        public string Password
        {
            set { _password = value; }
        }

        public string OAuthToken
        {
            get
            {
                if (_oAuthAppRegForm.LastAuthResult == null)
                    return null;
                return _oAuthAppRegForm.LastAuthResult.AccessToken;
            }
        }

        public bool OAuthTokenExpired()
        {
            if (_oAuthAppRegForm.LastAuthResult != null && _oAuthAppRegForm.LastAuthResult.ExpiresOn > DateTime.Now)
                return false;
            return true;
        }



        public void AcquireToken()
        {
            _oAuthAppRegForm.AcquireToken();
        }

        private bool HaveValidCredentials()
        {
            switch (_authType)
            {
                case AuthType.Default:
                    return true;

                case AuthType.Basic:
                    if (!String.IsNullOrEmpty(Username)) return true;
                    return false;

                case AuthType.Certificate:
                    return (_certificate != null);

                case AuthType.OAuth:
                    // Check if we already have valid access token
                    if (_oAuthAppRegForm.LastAuthResult != null && _oAuthAppRegForm.LastAuthResult.ExpiresOn > DateTime.Now) return true;
                    if (_oAuthAppRegForm.HaveValidAppConfig())
                    {
                        _oAuthAppRegForm.AcquireToken();
                        while (_oAuthAppRegForm.Authenticating)
                            System.Threading.Tasks.Task.Yield();
                    }
                    if (_oAuthAppRegForm.LastAuthResult != null && _oAuthAppRegForm.LastAuthResult.ExpiresOn > DateTime.Now) return true;
                    return false;

                default: return false;
            }
        }

        public bool ApplyCredentialsToHttpWebRequest(HttpWebRequest Request)
        {
            if (!HaveValidCredentials())
                return false;

            switch (_authType)
            {
                case AuthType.Default:
                    Request.UseDefaultCredentials = true;
                    return true;

                case AuthType.Basic:
                    Request.Credentials = new NetworkCredential(Username, _password);
                    return true;

                case AuthType.Certificate:
                    Request.ClientCertificates = new X509CertificateCollection();
                    Request.ClientCertificates.Add(_certificate);
                    return true;

                case AuthType.OAuth:
                    Request.Headers["Authorization"] = $"Bearer {_oAuthAppRegForm.LastAuthResult.AccessToken}";
                    return true;
            }

            return false;
        }

        public void UpdateOAuthCredentialsForExchangeService(ExchangeService Exchange)
        {
            if (_authType != AuthType.OAuth)
                return;
            if (_oAuthAppRegForm.LastAuthResult?.ExpiresOn > DateTime.Now) return;
            _oAuthAppRegForm.AcquireToken();
            while (_oAuthAppRegForm.Authenticating)
                System.Threading.Tasks.Task.Yield();
            if (_oAuthAppRegForm.AccessToken != null)
                Exchange.Credentials = new OAuthCredentials(_oAuthAppRegForm.AccessToken);
        }

        public bool ApplyCredentialsToExchangeService(ExchangeService Exchange)
        {
            if (!HaveValidCredentials())
                return false;

            switch (_authType)
            {
                case AuthType.Default:
                    Exchange.UseDefaultCredentials = true;
                    return true;

                case AuthType.Basic:
                    Exchange.Credentials = new NetworkCredential(Username, _password);
                    return true;

                case AuthType.Certificate:
                    //Request.ClientCertificates = new X509CertificateCollection();
                    //Request.ClientCertificates.Add(_certificate);
                    return false;

                case AuthType.OAuth:
                    while (_oAuthAppRegForm.Authenticating)
                        System.Threading.Tasks.Task.Yield();
                    if (_oAuthAppRegForm.AccessToken == null)
                        return false;
                    Exchange.Credentials = new OAuthCredentials(_oAuthAppRegForm.AccessToken);
                    return true;
            }

            return false;
        }

        public bool ApplyCredentialsToAutodiscoverService(AutodiscoverService Autodiscover)
        {
            if (!HaveValidCredentials())
                return false;

            switch (_authType)
            {
                case AuthType.Default:
                    Autodiscover.UseDefaultCredentials = true;
                    return true;

                case AuthType.Basic:
                    Autodiscover.Credentials = new NetworkCredential(Username, _password);
                    return true;

                case AuthType.Certificate:
                    //Request.ClientCertificates = new X509CertificateCollection();
                    //Request.ClientCertificates.Add(_certificate);
                    return false;

                case AuthType.OAuth:
                    Autodiscover.Credentials = new OAuthCredentials(_oAuthAppRegForm.LastAuthResult.AccessToken);
                    return true;
            }

            return false;
        }
    }
}
