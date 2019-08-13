using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using mARkIt.Abstractions;
using mARkIt.Authentication;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Xamarin.Auth;

namespace mARkIt.Services
{
    public class AzureService
    {
        private static string BackendURL = "https://mark-api.azurewebsites.net/";

        public static MobileServiceClient MobileService = new MobileServiceClient(BackendURL);

        public static bool IsConnected
        {
            get
            {
                return MobileService.CurrentUser != null;
            }
        }  

        public static async Task LoginToBackend(MobileServiceAuthenticationProvider i_AuthType, Account i_Account)
        {
            var zumoPayload = new JObject();
            zumoPayload.Add("access_token", i_Account.Properties["access_token"]);

            if (i_AuthType == MobileServiceAuthenticationProvider.Google)
            {
                zumoPayload.Add("id_token", i_Account.Properties["id_token"]);
            }

            await MobileService.LoginAsync(i_AuthType, zumoPayload);
        }

        public static async Task<bool> Insert<T>(T i_ObjectToInsert)
        {
            try
            {
                await MobileService.GetTable<T>().InsertAsync(i_ObjectToInsert);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> Delete<T>(T i_ObjectToDelete)
        {
            try
            {
                await MobileService.GetTable<T>().DeleteAsync(i_ObjectToDelete);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<T> GetById<T>(string i_Id) where T : TableData
        {
            try
            {
                var table = MobileService.GetTable<T>().Where(t => t.id == i_Id);
                var list = await table.ToListAsync();
                return list.First();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<bool> Update<T>(T i_ObjectToUpdate)
        {
            try
            {
                await MobileService.GetTable<T>().UpdateAsync(i_ObjectToUpdate);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task Logout()
        {
           await MobileService.LogoutAsync();
        }
    }
}
