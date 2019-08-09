using mARkIt.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace mARkIt.Authentication
{
    public class FacebookClient : AuthenticatorClient
    {
        public FacebookClient(Account i_Account) : base(i_Account)
        {
        }

        protected override string BuildUserRequestUri()
        {
            return $"https://graph.facebook.com/me?fields=first_name,last_name,email&access_token={m_AccessToken}";
        }
    }
}
