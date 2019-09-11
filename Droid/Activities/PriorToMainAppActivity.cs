using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using mARkIt.Droid.Services;

namespace mARkIt.Droid.Activities
{
    public abstract class PriorToMainAppActivity : AppCompatActivity
    {
        protected void StartMainApp()
        {
            AndroidNotifications.Register(context: this);

            Intent mainTabs = new Intent(this, typeof(TabsActivity));
            mainTabs.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            StartActivity(mainTabs);
            Finish();
        }
    }
}