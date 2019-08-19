using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace mARkIt.Backend.Notifications
{
    public class Notification
    {
        private const string appCenterApiToken = "2d1ebd8c1c907e453bb933b79c9d02a340247caf";
        private AndroidNotificationJson androidNotificationObj = new AndroidNotificationJson();
        private IOSNotificationJson iOSNotificationObj = new IOSNotificationJson();

        /// <summary>
        /// Determines if the notification will be sent to all the registered devices, or only to the listed Targets.
        /// </summary>
        public bool IsBroadcast
        {
            get
            {
                return androidNotificationObj.IsBroadcast;
            }

            set
            {
                androidNotificationObj.IsBroadcast = iOSNotificationObj.IsBroadcast = value;
            }
        }

        /// <summary>
        /// The official name of the notification, as it will be registered at the AppCenter logs.
        /// </summary>
        public string Name
        {
            get
            {
                return androidNotificationObj.Name;
            }

            set
            {
                androidNotificationObj.Name = iOSNotificationObj.Name = value;
            }
        }

        public string Title
        {
            get
            {
                return androidNotificationObj.Title;
            }

            set
            {
                androidNotificationObj.Title = iOSNotificationObj.Title = value;
            }
        }
        public string Body
        {
            get
            {
                return androidNotificationObj.Body;
            }

            set
            {
                androidNotificationObj.Body = iOSNotificationObj.Body = value;
            }
        }

        /// <summary>
        /// A list of target users,devices or accounts to which the notification will be sent.     
        /// </summary>
        /// <remarks>
        /// - "users" refers to custom made user identification which is set through the AppCenter SDK: AppCenter.SetUserId().
        /// - "devices" refers to a unique install id which is determined by AppCenter while registering. Obtained at the client via AppCenter.GetInstallIdAsync().
        /// - "accounts" - refers to user identity defined through AppCenter Auth.
        /// - Ignored if IsBroadcast == true.
        /// - The type of the targets should be only one of users/devices/accounts and updated accordingly in the property NotificationObject.TargetType.          
        /// </remarks>
        public List<string> Targets
        {
            get
            {
                return androidNotificationObj.Targets;
            }

            set
            {
                androidNotificationObj.Targets = iOSNotificationObj.Targets = value;
            }
        }

        /// <summary>
        /// Has to match the type of items in Targets.
        /// </summary>
        public eTargetType TargetType
        {
            get
            {
                return androidNotificationObj.TargetType;
            }

            set
            {
                androidNotificationObj.TargetType = iOSNotificationObj.TargetType = value;
            }
        }

        /// <summary>
        /// Additional data that can be extracted by the client while receiving the notification.
        /// </summary>
        public Dictionary<string, string> CustomData
        {
            get
            {
                return androidNotificationObj.CustomData;
            }

            set
            {
                androidNotificationObj.CustomData = iOSNotificationObj.CustomData = value;
            }
        }

        public async Task Push()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.appcenter.ms/v0.1/apps/MTA_FinalProject_mARk-It/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-API-Token", appCenterApiToken);

                if (IsBroadcast)
                {
                    await sendPushRequest(client);
                }
                else
                {
                    await sendInChunks(client);
                }
            }
        }

        private async Task sendInChunks(HttpClient client)
        {
            // Per API request AppCenter can only publish 100 user-targeted notifications, or 1000 specific device-targeted notifications
            int maxChunkSize = TargetType == eTargetType.Devices ? 1000 : 100;

            List<string> allTargets = Targets;

            while (allTargets.Count >= maxChunkSize)
            {
                Targets = allTargets.Take(100).ToList();
                allTargets.RemoveRange(index: 0, count: maxChunkSize);
                await sendPushRequest(client);
            }

            // Send the rest
            if (allTargets.Count > 0)
            {
                Targets = allTargets;
                await sendPushRequest(client);
            }
        }

        private async Task sendPushRequest(HttpClient client)
        {
            var content1 = new StringContent(androidNotificationObj.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage response1 = await client.PostAsync("mARk-It.Android/push/notifications", content1);

            // iOS notifications are not configured - payment is required  
            /// var content2 = new StringContent(iOSNotificationObj.ToString(), Encoding.UTF8, "application/json");
            /// HttpResponseMessage response1 = await client.PostAsync("mARk-It.iOS/push/notifications", content2);
        }
    }
}