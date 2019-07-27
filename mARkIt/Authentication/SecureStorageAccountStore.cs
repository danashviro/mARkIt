using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Essentials;

namespace MarkIt.Utils
{
    public static class SecureStorageAccountStore
    {
        //// Saving the account, need to convert it to json
        //public static async Task SaveAsync(Account i_Account, string i_ServiceType)
        //{
        //    // Find existing accounts for the service
        //    var accounts = await FindAccountsForServiceAsync(i_ServiceType);

        //    // Remove existing account with Id if exists
        //    accounts.RemoveAll(a => a.Username == account.Username);

        //    // Add account we are saving
        //    accounts.Add(i_Account);

        //    // Serialize all the accounts to json
        //    var json = JsonConvert.SerializeObject(accounts);

        //    // Securely save the accounts for the given service
        //    await SecureStorage.SetAsync(i_ServiceType, json);
        //}

        //public static async Task<List<Account>> FindAccountsForServiceAsync(string i_ServiceType)
        //{
        //    // Get the json for accounts for the service
        //    var json = await SecureStorage.GetAsync(i_ServiceType);

        //    try
        //    {
        //        // Try to return deserialized list of accounts
        //        return JsonConvert.DeserializeObject<List<Account>>(json);
        //    }
        //    catch { }

        //    // If this fails, return an empty list
        //    return new List<Account>();
        //}

        public static async Task SaveAccountAsync(Account i_Account, string i_ServiceType)
        {
            // remove an already existing account of this service type if exists
            SecureStorage.Remove(i_ServiceType);

            // convert the account to json format
            // we will put it in a list so it will be convertiable
            List<Account> listOfAccounts = new List<Account>();
            listOfAccounts.Add(i_Account);
            string listOfAccountsInJsonFormat = JsonConvert.SerializeObject(listOfAccounts);

            // Securely save the accounts for the given service
            await SecureStorage.SetAsync(i_ServiceType, listOfAccountsInJsonFormat);
        }

        // return a stored account based on the given service type
        // returns null if there is no account stored for this service
        public static async Task<Account> GetAccountAsync(string i_ServiceType)
        {
            string accountInJsonFormat = await SecureStorage.GetAsync(i_ServiceType);
            List<Account> listOfAccounts = null;
            Account accountToReturn = null;
            // try to deserialize the json in Account object
            try
            {
                listOfAccounts = JsonConvert.DeserializeObject<List<Account>>(accountInJsonFormat);
            }
            catch (Exception)
            {
                listOfAccounts = null;
            }
            
            if (listOfAccounts != null && listOfAccounts.Count > 0)
            {
                accountToReturn = listOfAccounts[0];
            }
            return accountToReturn;
        }
    }
}