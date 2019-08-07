using mARkIt.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace mARkIt.Authentication
{
    public class GoogleClient
    {
        private HttpClient m_HttpClient;
        private string m_AccessToken = string.Empty;
        private string m_TokenType = string.Empty;

        public GoogleClient(Account i_Account)
        {
            if (i_Account != null)
            {
                m_AccessToken = i_Account.Properties["access_token"];
                m_TokenType = i_Account.Properties["token_type"];
                m_HttpClient = new HttpClient();
            }
        }

        public async Task<User> GetUserAsync()
        {
            string uri = new StringBuilder().AppendFormat("https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token={0}", m_AccessToken).ToString();
            string jsonResponse = await m_HttpClient.GetStringAsync(uri);
            User user = JsonConvert.DeserializeObject<User>(jsonResponse);
            return user;
        }
    }
}
