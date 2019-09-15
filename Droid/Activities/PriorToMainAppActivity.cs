using Android.Content;
using Android.Support.V7.App;
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