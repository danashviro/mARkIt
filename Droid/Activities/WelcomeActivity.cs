using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Xamarin.Auth;

namespace mARkIt.Droid.Activities
{
    [Activity(Label = "Welcome", MainLauncher = true)]
    public class WelcomeActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Task.Run(() => autoConnect());
            SetContentView(Resource.Layout.Welcome);
            //Finish();
        }

        private async void autoConnect()
        {
            Account account = null;
            account = await mARkIt.Authentication.SecureStorageAccountStore
                .GetAccountAsync("Facebook");
            if (account == null)
            {
                account = await mARkIt.Authentication.SecureStorageAccountStore
                    .GetAccountAsync("Google");
            }
            await Task.Delay(3000);
            Console.WriteLine("after delay");
            //await Task.Delay(TimeSpan.FromSeconds(5));
            // Go straight to main tabs page
            if (account != null)
            {
                startMainApp(account);
            }
            else // go to login page
            {
                startLoginPage();
            }
        }

        private void startLoginPage()
        {
            //Intent loginIntent = new Intent(this, typeof(LoginActivity));
            //StartActivity(loginIntent);
            //Finish();
        }

        private void startMainApp(Account i_Account)
        {
            //mARkIt.Authentication.FacebookClient fbClient = new Authentication.FacebookClient(i_Account);
            //fbClient.GetEmailAddress();
            //throw new NotImplementedException();
        }
    }
}