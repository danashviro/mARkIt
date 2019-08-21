using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace mARkIt.Backend.Notifications
{
    public class NotificationJson
    {
        private NotificationTarget m_Target;

        [JsonProperty("notification_target")]
        private NotificationTarget Target
        {
            get
            {
                if (IsBroadcast)
                {
                    // Don't serialize this property
                    return null;
                }
                else
                {
                    return m_Target;
                }
            }

            set
            {
                m_Target = value;
            }
        }

        [JsonProperty("notification_content")]
        private NotificationContent Content { get; set; }

        /// <summary>
        /// Determines whether the notification will be sent to every registered device, or by the assigned targets.
        /// </summary>
        [JsonIgnore]
        public bool IsBroadcast { get; set; } = false;

        /// <summary>
        /// A list of target users/devices/accounts to which the notification will be sent.     
        /// </summary>
        /// <remarks>
        /// - Ignored if IsBroadcast == true.
        /// - The type of the targets should be only one of users/devices/accounts and updated accordingly in the property NotificationObject.TargetType.
        /// </remarks>
        [JsonIgnore]
        public List<string> Targets { get; set; }

        private eTargetType m_TargetType;

        [JsonIgnore]
        public eTargetType TargetType
        {
            get
            {
                return m_TargetType;
            }

            set
            {
                m_TargetType = value;

                // Match to the accepted API format
                switch (value)
                {
                    case eTargetType.Users:
                        m_Target.Type = "user_ids_target";
                        break;
                    case eTargetType.Devices:
                        m_Target.Type = "devices_target";
                        break;
                    case eTargetType.Accounts:
                        m_Target.Type = "account_ids_target";
                        break;
                }
            }
        }

        [JsonIgnore]
        public string Name
        {
            get
            {
                return Content.Name;
            }

            set
            {
                Content.Name = value;
            }
        }

        [JsonIgnore]
        public string Title
        {
            get
            {
                return Content.Title;
            }

            set
            {
                Content.Title = value;
            }
        }

        [JsonIgnore]
        public string Body
        {
            get
            {
                return Content.Body;
            }

            set
            {
                Content.Body = value;
            }
        }

        [JsonIgnore]
        public IDictionary<string, string> CustomData
        {
            get
            {
                return Content.CustomData;
            }

            private set
            {
                Content.CustomData = value;
            }
        }

        public NotificationJson()
        {
            Target = new NotificationTarget();
            Content = new NotificationContent();
            Targets = new List<string>();
            TargetType = eTargetType.Users;
            CustomData = new Dictionary<string, string>();
        }

        public override string ToString()
        {
            adjustToTargetsType();

            // Don't serialize this property if it's empty
            if(CustomData.Count == 0)
            {
                CustomData = null;
            }

            var serializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };
            var notificationJObject = JObject.FromObject(this, serializer);

            // Restore to the previous state in case null was assigned
            if(CustomData == null)
            {
                CustomData = new Dictionary<string, string>();
            }

            return notificationJObject.ToString();
        }


        /// <summary>
        /// To serialize the object in the accepted API format - assign Targets to the matching property while setting null to the others (which disables their serialization).
        /// </summary>
        private void adjustToTargetsType()
        {
            if (Target != null)
            {
                switch (TargetType)
                {
                    case eTargetType.Users:
                        Target.Users = Targets;
                        Target.Devices = Target.Accounts = null;
                        break;
                    case eTargetType.Devices:
                        Target.Devices = Targets;
                        Target.Users = Target.Accounts = null;
                        break;
                    case eTargetType.Accounts:
                        Target.Accounts = Targets;
                        Target.Users = Target.Devices = null;
                        break;
                }
            }
        }

        #region private class definitions for json serialization

        private class NotificationTarget
        {
            [JsonProperty("type")]
            public string Type { get; set; } 

            [JsonProperty("user_ids")]
            public List<string> Users { get; set; }

            [JsonProperty("devices")]
            public List<string> Devices { get; set; }

            [JsonProperty("account_ids")]
            public List<string> Accounts { get; set; }
        }

        private class NotificationContent
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("body")]
            public string Body { get; set; }

            [JsonProperty("custom_data")]
            public IDictionary<string, string> CustomData { get; set; }

            public NotificationContent()
            {
                Name = Title = Body = string.Empty;
                CustomData = new Dictionary<string, string>();
            }
        }

        #endregion
    }
}