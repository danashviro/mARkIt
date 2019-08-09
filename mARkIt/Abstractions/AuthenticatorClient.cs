using mARkIt.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace mARkIt.Authentication
{
    public abstract class AuthenticatorClient
    {
        protected HttpClient m_HttpClient;
        protected string m_AccessToken = string.Empty;

        public AuthenticatorClient(Account i_Account)
        {
            if (i_Account != null)
            {
                m_HttpClient = new HttpClient();
                m_AccessToken = i_Account.Properties["access_token"];
            }
        }

        protected abstract string BuildUserRequestUri();

        public async Task<User> GetUserAsync()
        {
            string uri = BuildUserRequestUri();
            string jsonResponse = await m_HttpClient.GetStringAsync(uri);
            User user = JsonConvert.DeserializeObject<User>(jsonResponse);
            return user;
        }
    }
}