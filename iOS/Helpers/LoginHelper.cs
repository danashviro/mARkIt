using System;
using System.Threading.Tasks;
using mARkIt.Authentication;
using mARkIt.Models;
using Xamarin.Auth;

namespace mARkIt.iOS.Helpers
{
    public class LoginHelper
    {
        public static async Task<User> CreateUserObject(Account i_Account)
        {
            FacebookClient fbClient = new FacebookClient(i_Account);
            User user = await fbClient.GetUserAsync();
            return await User.GetUserByEmail(user.Email);
        }
    }
}
