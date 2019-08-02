using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            throw new NotImplementedException();
        }
    }
}
