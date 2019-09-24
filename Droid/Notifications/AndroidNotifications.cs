using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using mARkIt.Services;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;
using System;
using System.Threading.Tasks;
using mARkIt.Droid.Activities;
using Plugin.Geolocator.Abstractions;
using mARkIt.Models;
using System.Net.Http;
using System.Collections.Generic;
using Plugin.Geolocator;
using mARkIt.Authentication;

namespace mARkIt.Droid.Notifications
{
    public static class AndroidNotifications
    {
        private static bool isRegistered = false;
        private static Context Context { get; set; }

        public static void Register(Context context)
        {
            if (!isRegistered)
            {
                Context = context;
                Task.Run(() => setupAppCenterPush());
                searchForNewClosestMarkInTheBackground();
                LoginHelper.LoggedIn += searchForNewClosestMarkInTheBackground;
                LoginHelper.LoggedOut += stopSearching;

                isRegistered = true;
            }            
        }

        private static void onLoggedIn()
        {
            searchForNewClosestMarkInTheBackground();
        }        

        private static void stopSearching()
        {
            m_ShouldContinueSearching = false;
        }

        private async static void setupAppCenterPush()
        {
            const string androidAppSecret = "7d213151-6a71-406c-9db2-732e5fa1b464";

            AppCenter.Start(androidAppSecret, typeof(Push));

            Guid? install_guid = await AppCenter.GetInstallIdAsync();

            if (install_guid.HasValue)
            {
                await AzureWebApi.RegisterNotificationsId(install_guid.Value.ToString());

                // Register the new user after logging out the previous one.
                App.UserChanged += async (user) => await AzureWebApi.RegisterNotificationsId(install_guid.Value.ToString());

                Push.PushNotificationReceived += OnNotificationReceived;
            }
        }

        private static void OnNotificationReceived(object sender, PushNotificationReceivedEventArgs e)
        {
            var resultIntent = determineResultIntent(e);
            new LocalNotification(Context).Show(e.Title, e.Message, resultIntent);
        }

        private static Intent determineResultIntent(PushNotificationReceivedEventArgs e)
        {
            // When the user clicks the notification, TabsActivity will start up.
            return new Intent(Context, typeof(TabsActivity));
        }

        private static bool m_ShouldContinueSearching = true;

        private static void searchForNewClosestMarkInTheBackground()
        {
            m_ShouldContinueSearching = true;
            PowerManager pw = (PowerManager)Context.GetSystemService(Context.PowerService);
            PowerManager.WakeLock wl = pw.NewWakeLock(WakeLockFlags.Full, "MarkitWakelock");
            wl.Acquire();

            var stam = Task.Run(() =>
            {
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = TimeSpan.FromSeconds(60).TotalMilliseconds;
                timer.Enabled = true;
                timer.Elapsed += (s, e) =>
                {
                    checkForClosestNewMark();

                    if (!m_ShouldContinueSearching)
                    {
                        timer.Stop();
                        wl.Release();
                    }
                };

                timer.Start();
            });
        }

        public static void StopLongRunningBackgroundTask()
        {
            m_ShouldContinueSearching = false;
        }

        private async static void checkForClosestNewMark()
        {
            try
            {
                Position lastKnownPosition = await CrossGeolocator.Current.GetLastKnownLocationAsync();

                if (lastKnownPosition != null)
                {
                    Dictionary<string, string> parameters = new Dictionary<string, string>
                    {
                        {"latitude", lastKnownPosition.Latitude.ToString() },
                        {"longitude", lastKnownPosition.Longitude.ToString() }
                    };

                    Mark closestMark = await AzureWebApi.MobileService.InvokeApiAsync<Mark>("ClosestMark", HttpMethod.Get, parameters);

                    if (closestMark != null)
                    {
                        new LocalNotification(Context).Show("mARkIt", "A new mark is closeby!");
                    }
                }
            }

            catch (Exception e)
            {
                Android.Util.Log.Debug("MyWorker", "failure:");
                Android.Util.Log.Debug("MyWorker", e.Message);
            }
        }

        ////public static void setupWorker()
        ////{
        ////    WorkManager.Instance.CancelAllWork();

        ////    //var constraints = new Constraints.Builder().SetRequiredNetworkType(NetworkType.Connected).Build();
        ////    var data = new Data.Builder().PutString("UserId", App.ConnectedUser.Id).Build();

        ////    PeriodicWorkRequest workRequest = PeriodicWorkRequest.Builder.From<CheckForNewMarksWorker>(TimeSpan.FromMinutes(15))
        ////        //.SetConstraints(constraints)
        ////        .SetInputData(data)
        ////        .Build();

        ////    WorkManager.Instance.Enqueue(workRequest);
        ////}
    }
}