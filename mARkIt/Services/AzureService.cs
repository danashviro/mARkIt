using System;
using System.Threading.Tasks;
using mARkIt.Abstractions;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using System.Linq;

namespace mARkIt.Services
{
    public class AzureService
    {
        private static string BackendURL = "https://mark-api.azurewebsites.net/";

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

        public static async Task<T> GetById<T>(string i_Id) where T : TableData
        {
            try
            {
                var table = MobileService.GetTable<T>().Where(t => t.id == i_Id);
                var list=await table.ToListAsync();
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
    }
}
