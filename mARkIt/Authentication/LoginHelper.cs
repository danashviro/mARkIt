using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using mARkIt.Models;
using mARkIt.Services;
using mARkIt.Utils;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Auth;

namespace mARkIt.Authentication
{
    public class LoginHelper
    {
        public static GoogleAuthenticator s_GoogleAuthenticator;
        public static FacebookAuthenticator s_FacebookAuthenticator;
        public static MobileServiceAuthenticationProvider s_AuthType;

        private static async Task CreateUserObjectAsync(Account i_Account, Func<Account, Task> i_GoogleRefreshTokenFunc = null)
        {
            User user;

            switch (s_AuthType)
            {
                case MobileServiceAuthenticationProvider.Facebook:
                    FacebookClient fbClient = new FacebookClient(i_Account);
                    user = await fbClient.GetUserAsync();
                    break;
                case MobileServiceAuthenticationProvider.Google:
                    try
                    {
                        GoogleClient glClient = new GoogleClient(i_Account);
                        user = await glClient.GetUserAsync();
                    }
                    catch (Exception)
                    {
                        if (i_GoogleRefreshTokenFunc != null)
                        {
                            await i_GoogleRefreshTokenFunc(i_Account);
                            try
                            {   // retry after refreshing
                                GoogleClient glClient = new GoogleClient(i_Account);
                                user = await glClient.GetUserAsync();
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                    }
                    break;
            }

            await AzureService.LoginToBackend(s_AuthType, i_Account);
        }


        public static async Task CreateUserAndSaveToDevice(Account i_Account, Func<Account, Task> i_GoogleRefreshTokenFunc = null)
        {
            // save account to device
            if (s_FacebookAuthenticator != null)
            {
                await SecureStorageAccountStore.SaveAccountAsync(i_Account, "Facebook");
                s_AuthType = MobileServiceAuthenticationProvider.Facebook;
            }
            else if (s_GoogleAuthenticator != null)
            {
                await SecureStorageAccountStore.SaveAccountAsync(i_Account, "Google");
                s_AuthType = MobileServiceAuthenticationProvider.Google;
            }
            await CreateUserObjectAsync(i_Account, i_GoogleRefreshTokenFunc);
        }


        public static async Task AutoConnect(Func<Account, Task> i_GoogleRefreshTokenFunc)
        {
            Account account;
            account = await SecureStorageAccountStore.GetAccountAsync("Facebook");
            s_AuthType = MobileServiceAuthenticationProvider.Facebook;
            if (account == null)
            {
                account = await SecureStorageAccountStore.GetAccountAsync("Google");
                s_AuthType = MobileServiceAuthenticationProvider.Google;
            }

            if (account != null)
            {
                await CreateUserObjectAsync(account, i_GoogleRefreshTokenFunc);
            }
        }

        public static OAuth2Authenticator GetFacebook2Authenticator(IAuthenticationDelegate i_AuthenticationDelegate)
        {
            s_FacebookAuthenticator = new FacebookAuthenticator(Keys.FacebookAppId, Configuration.FacebookAuthScope, i_AuthenticationDelegate);
            s_GoogleAuthenticator = null;
            return s_FacebookAuthenticator.GetOAuth2();
        }

        public static OAuth2Authenticator GetGoogle2Authenticator(IAuthenticationDelegate i_AuthenticationDelegate)
        {
            s_GoogleAuthenticator = new GoogleAuthenticator(Keys.GoogleClientId, Configuration.GoogleAuthScope, i_AuthenticationDelegate);
            s_FacebookAuthenticator = null;
            return s_GoogleAuthenticator.GetOAuth2();
        }

        public static async Task Logout()
        {
            Xamarin.Essentials.SecureStorage.RemoveAll();
            await AzureService.Logout();
        }
    }
}