using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace mARkIt.Services
{
    public class AzureService
    {
        private static string BackendURL = "http://mark-api.azurewebsites.net/";

        public static MobileServiceClient MobileService = new MobileServiceClient(BackendURL);

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
    }
}
