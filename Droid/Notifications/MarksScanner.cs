using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using mARkIt.Services;
using System;
using System.Threading.Tasks;
using Plugin.Geolocator.Abstractions;
using mARkIt.Models;
using System.Net.Http;
using System.Collections.Generic;
using Plugin.Geolocator;

namespace mARkIt.Droid.Notifications
{
    public static class MarksScanner
    {
        private static System.Timers.Timer m_Timer;
        private static PowerManager.WakeLock m_WakeLock;

        public static void StartScanning(Context context)
        {
            // Prevent the Android OS from killing our process while it is in the background
            PowerManager pw = (PowerManager)context.GetSystemService(Context.PowerService);
            m_WakeLock = pw.NewWakeLock(WakeLockFlags.Full, "MarkitWakelock");
            m_WakeLock.Acquire();

            // Poll the web server every minute for new marks to notify about
            var stam = Task.Run(() =>
            {
                m_Timer = new System.Timers.Timer();
                m_Timer.Interval = TimeSpan.FromSeconds(60).TotalMilliseconds;
                m_Timer.Enabled = true;
                m_Timer.Elapsed += (s, e) => checkForClosestNewMark(context);
                m_Timer.Start();
            });
        }

        public static void StopScanning()
        {
            if (m_Timer != null && m_WakeLock != null)
            {
                m_Timer.Stop();
                m_WakeLock.Release();
            }
        }

        private async static void checkForClosestNewMark(Context context)
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
                        new LocalNotification(context).Show("mARkIt", "A new mark is closeby!");
                    }
                }
            }

            catch (Exception e)
            {
                Android.Util.Log.Debug("Markit", e.Message);
            }
        }
    }
}