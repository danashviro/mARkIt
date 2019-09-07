using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using mARkIt.Services;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;
using System;
using System.Threading.Tasks;

namespace mARkIt.Droid.Services
{
    public static class PushNotificationsService
    {
        // Unique ID for our notification: 
        static readonly int NOTIFICATION_ID = 1000;
        static readonly string CHANNEL_ID = "location_notification";
        private static bool isRegistered = false;
        private static Context Context { get; set; }

        public static void Register(Context context)
        {
            if (!isRegistered)
            {
                Context = context;
                Task.Run(()=>setupAppCenterPush());
                isRegistered = true;
            }
        }

        private async static void setupAppCenterPush()
        {
            const string androidAppSecret = "7d213151-6a71-406c-9db2-732e5fa1b464";

            AppCenter.Start(androidAppSecret, typeof(Push));

            Guid? install_guid = await AppCenter.GetInstallIdAsync();

            if (install_guid.HasValue)
            {
                await AzureService.RegisterNotificationsId(install_guid.Value.ToString());

                // Register the new user after logging out the previous one.
                App.UserChanged += async (user) => await AzureService.RegisterNotificationsId(install_guid.Value.ToString());

                Push.PushNotificationReceived += OnNotificationReceived;
            }
        }

        private static void OnNotificationReceived(object sender, PushNotificationReceivedEventArgs e)
        {
            createNotificationChannel();
            var resultIntent = determineResultIntent(e);
            DisplayNotification(e.Title, e.Message, resultIntent);
        }

        public static void createNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification 
                // channel on older versions of Android.
                return;
            }

            var name = "mARK-It Notification";
            var channel = new NotificationChannel(CHANNEL_ID, name, NotificationImportance.High)
            {
                Description = ""
            };

            var notificationManager = (NotificationManager)Context.GetSystemService(Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        private static Intent determineResultIntent(PushNotificationReceivedEventArgs e)
        {
            // When the user clicks the notification, TabsActivity will start up.
            return new Intent(Context, typeof(TabsActivity));
        }

        public static void DisplayNotification(string title, string message, Intent resultIntent)
        {
            // Construct a back stack for cross-task navigation:
            var stackBuilder = Android.App.TaskStackBuilder.Create(Context);
            stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(TabsActivity)));
            stackBuilder.AddNextIntent(resultIntent);

            // Create the PendingIntent with the back stack:            
            var resultPendingIntent = stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);

            // Build the notification:
            var builder = new NotificationCompat.Builder(Context, CHANNEL_ID)
                          .SetAutoCancel(true) // Dismiss the notification from the notification area when the user clicks on it
                          .SetContentIntent(resultPendingIntent) // Start up this activity when the user clicks the intent.
                          .SetContentTitle(title) // Set the title
                          .SetSmallIcon(Resource.Drawable.MainLogo)
                          .SetContentText(message); // the message to display.

            // Finally, publish the notification:
            var notificationManager = NotificationManagerCompat.From(Context);
            notificationManager.Notify(NOTIFICATION_ID, builder.Build());
        }
    }
}