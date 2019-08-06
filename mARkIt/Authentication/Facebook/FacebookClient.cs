using mARkIt.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace mARkIt.Authentication
{
    public class FacebookClient
    {
        private HttpClient m_HttpClient;
        private string m_AccessToken = string.Empty;

        public FacebookClient(Account i_Account)
        {
            if (i_Account != null)
            {
                m_AccessToken = i_Account.Properties["access_token"];
                m_HttpClient = new HttpClient();
            }
        }

        public async Task<User> GetUserAsync()
        {
            string uri = new StringBuilder().AppendFormat("https://graph.facebook.com/me?fields=first_name,last_name,email&access_token={0}", m_AccessToken).ToString();
            string jsonResponse = await m_HttpClient.GetStringAsync(uri);
            User user = JsonConvert.DeserializeObject<User>(jsonResponse);
            return user;
        }
    }
}
