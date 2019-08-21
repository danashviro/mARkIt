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
    /// <summary>
    /// Defines and pushes a notification to Android and iOS devices via AppCenter configuration.
    /// </summary>
    public abstract class Notification
    {
        private string m_ApiToken;
        private string m_BaseUrl;
        private string m_AndroidUrl;
        private string m_iOSUrl;
        private Dictionary<string, string> m_OriginalCustomDataAndroid;
        private Dictionary<string, string> m_OriginalCustomDataIOS;
        protected abstract NotificationJson AndroidNotificationObj { get; }
        protected abstract NotificationJson iOSNotificationObj { get; }

        /// <summary>
        /// Determines if the notification will be sent to all the registered devices, or only to the listed Targets.
        /// </summary>
        public bool IsBroadcast
        {
            get
            {
                return AndroidNotificationObj.IsBroadcast;
            }

            set
            {
                AndroidNotificationObj.IsBroadcast = iOSNotificationObj.IsBroadcast = value;
            }
        }

        /// <summary>
        /// The official name of the notification, as it will be registered at the AppCenter logs.
        /// </summary>
        public string Name
        {
            get
            {
                return AndroidNotificationObj.Name;
            }

            set
            {
                AndroidNotificationObj.Name = iOSNotificationObj.Name = value;
            }
        }

        public string Title
        {
            get
            {
                return AndroidNotificationObj.Title;
            }

            set
            {
                AndroidNotificationObj.Title = iOSNotificationObj.Title = value;
            }
        }
        public string Body
        {
            get
            {
                return AndroidNotificationObj.Body;
            }

            set
            {
                AndroidNotificationObj.Body = iOSNotificationObj.Body = value;
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
                return AndroidNotificationObj.Targets;
            }

            set
            {
                AndroidNotificationObj.Targets = iOSNotificationObj.Targets = value;
            }
        }

        /// <summary>
        /// Has to match the type of items in Targets.
        /// </summary>
        public eTargetType TargetType
        {
            get
            {
                return AndroidNotificationObj.TargetType;
            }

            set
            {
                AndroidNotificationObj.TargetType = iOSNotificationObj.TargetType = value;
            }
        }

        /// <summary>
        /// Additional data that can be extracted by the client while receiving the notification.
        /// </summary>
        public IDictionary<string, string> AdditionalData { get; private set; }

        public Notification(
            string i_AppCenterApiToken,
            string i_AppCenterOrganizationName,
            string i_AppCenterAndroidProjectName,
            string i_AppCenterIOSProjectName)
        {
            m_ApiToken = i_AppCenterApiToken;
            m_BaseUrl = $"https://api.appcenter.ms/v0.1/apps/{i_AppCenterOrganizationName}/";
            m_AndroidUrl = $"{i_AppCenterAndroidProjectName}/push/notifications";
            m_iOSUrl = $"{i_AppCenterIOSProjectName}/push/notifications";

            AdditionalData = new Dictionary<string, string>();                
        }

        /// <summary>
        /// Pushes the notification to Android and iOS devices according to the configuration
        /// </summary>
        /// <remarks>
        /// Due to targets count, the notification may be sent over multiple http requests.
        /// </remarks>
        /// <returns>
        /// A touple of http response messages from the last http request:
        /// The first response is from the request to push to Android,
        /// the second response is from the request to push to iOS.
        /// </returns>
        public async Task<Tuple<HttpResponseMessage,HttpResponseMessage>> Push()
        {
            Tuple<HttpResponseMessage, HttpResponseMessage> responses = null;

            addAdditionalDataToCustomData();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(m_BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-API-Token", m_ApiToken);

                if (IsBroadcast)
                {
                    responses = await sendPushRequest(client);
                }
                else
                {
                    responses = await sendInChunks(client);
                }
            }

            // For accurate reflection of the changes in AdditionalData between pushes
            revertCustomDataChanges();

            return responses;
        }

        private async Task<Tuple<HttpResponseMessage, HttpResponseMessage>> sendInChunks(HttpClient i_HttpClient)
        {
            Tuple<HttpResponseMessage, HttpResponseMessage> responses = null;

            // Per API request AppCenter can only publish 100 user-targeted notifications, or 1000 specific device-targeted notifications
            int maxChunkSize = TargetType == eTargetType.Devices ? 1000 : 100;

            List<string> allTargets = Targets;

            while (allTargets.Count >= maxChunkSize)
            {
                Targets = allTargets.Take(100).ToList();
                allTargets.RemoveRange(index: 0, count: maxChunkSize);
                responses = await sendPushRequest(i_HttpClient);
            }

            // Send the rest
            if (allTargets.Count > 0)
            {
                Targets = allTargets;
                responses = await sendPushRequest(i_HttpClient);
            }

            return responses;
        }

        private async Task<Tuple<HttpResponseMessage, HttpResponseMessage>> sendPushRequest(HttpClient i_HttpClient)
        {
            var androidPushContent = new StringContent(AndroidNotificationObj.ToString(), Encoding.UTF8, "application/json");
            var androidPushResponse = await i_HttpClient.PostAsync(m_AndroidUrl, androidPushContent);

            var iOSPushContent = new StringContent(iOSNotificationObj.ToString(), Encoding.UTF8, "application/json");
            var iOSPushResponse = await i_HttpClient.PostAsync(m_iOSUrl, iOSPushContent);

            return Tuple.Create(androidPushResponse, iOSPushResponse);
        }

        private void addAdditionalDataToCustomData()
        {
            IDictionary<string, string> androidCustomData = AndroidNotificationObj.CustomData;
            IDictionary<string, string> iOSCustomData = iOSNotificationObj.CustomData;

            m_OriginalCustomDataAndroid = androidCustomData.ToDictionary(entry => entry.Key, entry => entry.Value);
            m_OriginalCustomDataIOS = iOSCustomData.ToDictionary(entry => entry.Key, entry => entry.Value);

            foreach (KeyValuePair<string, string> entry in AdditionalData)
            {
                if (!androidCustomData.ContainsKey(entry.Key))
                {
                    androidCustomData.Add(entry.Key, entry.Value);
                }

                if (!iOSCustomData.ContainsKey(entry.Key))
                {
                    iOSCustomData.Add(entry.Key, entry.Value);
                }
            }
        }

        private void revertCustomDataChanges()
        {
            IDictionary<string, string> androidCustomData = AndroidNotificationObj.CustomData;
            IDictionary<string, string> iOSCustomData = iOSNotificationObj.CustomData;

            androidCustomData.Clear();
            iOSCustomData.Clear();

            foreach (KeyValuePair<string, string> entry in m_OriginalCustomDataAndroid)
            {
                androidCustomData.Add(entry.Key, entry.Value);
            }

            foreach (KeyValuePair<string, string> entry in m_OriginalCustomDataIOS)
            {
                iOSCustomData.Add(entry.Key, entry.Value);
            }
        }
    }
}