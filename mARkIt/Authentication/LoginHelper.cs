using System;
using System.Threading.Tasks;
using mARkIt.Models;
using mARkIt.Utils;
using Xamarin.Auth;

namespace mARkIt.Authentication
{
    public class LoginHelper
    {
        public static GoogleAuthenticator s_GoogleAuthenticator;
        public static FacebookAuthenticator s_FacebookAuthenticator;

        public static async Task<User> CreateUserObject(Account i_Account, Authentication.e_SupportedAuthentications i_AuthType)
        {
            User user = null;
            switch (i_AuthType)
            {
                case Authentication.e_SupportedAuthentications.Facebook:
                    FacebookClient fbClient = new FacebookClient(i_Account);
                    user = await fbClient.GetUserAsync();
                    break;
                case Authentication.e_SupportedAuthentications.Google:
                    GoogleClient glClient = new GoogleClient(i_Account);
                    user = await glClient.GetUserAsync();
                    break;
            }

            return await User.GetUserByEmail(user.Email);
        }

        public static async Task<User> GetUser(Account i_Account)
        {
            Authentication.e_SupportedAuthentications authType = Authentication.e_SupportedAuthentications.Facebook;
            // save account to device
            if (s_FacebookAuthenticator != null)
            {
                await SecureStorageAccountStore.SaveAccountAsync(i_Account, "Facebook");
                authType = Authentication.e_SupportedAuthentications.Facebook;
            }
            else if (s_GoogleAuthenticator != null)
            {
                await SecureStorageAccountStore.SaveAccountAsync(i_Account, "Google");
                authType = Authentication.e_SupportedAuthentications.Google;
            }

            return await CreateUserObject(i_Account, authType);
        }

        public static async Task<Account> AutoConnect()
        {
            string authType;
            Account account;
            account = await SecureStorageAccountStore.GetAccountAsync("Facebook");
            authType = "Facebook";
            if (account == null)
            {
                account = await SecureStorageAccountStore.GetAccountAsync("Google");
                authType = "Google";
            }
            if (account != null)
            {
                account.Properties.Add("AuthType", authType);
            }

            return account;
        }

        public static OAuth2Authenticator GetFacebook2Authenticator(IAuthenticationDelegate i_AuthenticationDelegate)
        {
            s_FacebookAuthenticator = new FacebookAuthenticator(Keys.FacebookAppId,Configuration.FacebookAuthScope, i_AuthenticationDelegate);
            return s_FacebookAuthenticator.GetOAuth2();
        }

        public static OAuth2Authenticator GetGoogle2Authenticator(IAuthenticationDelegate i_AuthenticationDelegate)
        {
            s_GoogleAuthenticator = new GoogleAuthenticator( Keys.GoogleClientId,Configuration.GoogleAuthScope, i_AuthenticationDelegate);
            return s_GoogleAuthenticator.GetOAuth2();
        }
    }
}
