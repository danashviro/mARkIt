using mARkIt.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace mARkIt.Authentication
{
    public class GoogleClient : AuthenticatorClient
    {
        public GoogleClient(Account i_Account) : base(i_Account)
        {
        }

        protected override string BuildUserRequestUri()
        {
            return $"https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token={m_AccessToken}";
        }
    }
}
