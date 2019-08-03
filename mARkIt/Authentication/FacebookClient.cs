using System;
using Xamarin.Auth;

namespace mARkIt.Authentication
{
    public class FacebookClient
    {
        private string m_AccessToken = string.Empty;

        public FacebookClient(Account i_Account)
        {
            m_AccessToken = i_Account.Properties["access_token"];
        }

        public string GetEmailAddress()
        {
            // GET(acces_token, "email")
            throw new NotImplementedException();
        }
    }
}
