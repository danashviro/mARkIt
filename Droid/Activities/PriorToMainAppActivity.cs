using Android.Content;
using Android.Support.V7.App;
using mARkIt.Authentication;
using mARkIt.Droid.Notifications;

namespace mARkIt.Droid.Activities
{
    public abstract class PriorToMainAppActivity : AppCompatActivity
    {
        protected void StartMainApp()
        {
            MarksScanner.StartScanning(context: this);
            LoginHelper.LoggedOut += Notifications.MarksScanner.StopScanning;

            Intent mainTabs = new Intent(this, typeof(TabsActivity));
            mainTabs.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            StartActivity(mainTabs);
            Finish();
        }
    }
}