using mARkIt.Abstractions;
using mARkIt.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace mARkIt.Models
{
    public class User : TableData
    {
        public User()
        { }

        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public int RelevantCategoriesCode { get; set; }

        public static async Task<bool> Insert(User i_User)
        {
            return await AzureService.Insert(i_User);
        }

        public static async Task<bool> Delete(User i_User)
        {
            return await AzureService.Delete(i_User);
        }

        public static async Task<bool> Update(User i_User)
        {
            return await AzureService.Update(i_User);
        }

        public static async Task<User> GetUserByEmail(string email)
        {
            List<User> users= await AzureService.MobileService.GetTable<User>().Where(u => u.Email == email).ToListAsync();
            if(users.Count!=0)
            {
                return users.First();
            }
            else
            {
                User user = new User()
                {
                    Email = email,
                    RelevantCategoriesCode = 0x1F
                };
                await Insert(user);
                users = await AzureService.MobileService.GetTable<User>().Where(u => u.Email == email).ToListAsync();
                return users.First();
            }
        }
    }
}
