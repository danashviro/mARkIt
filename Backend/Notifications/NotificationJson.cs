using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace mARkIt.Backend.Notifications
{
    public abstract class NotificationJson
    {
        private eTargetType targetType = eTargetType.Users;

        [JsonIgnore]
        public eTargetType TargetType
        {
            get
            {
                return targetType;
            }

            set
            {
                targetType = value;

                // Match to the accepted API format
                switch (value)
                {
                    case eTargetType.Users:
                        notificationTarget.Type = "user_ids_target";
                        break;
                    case eTargetType.Devices:
                        notificationTarget.Type = "devices_target";
                        break;
                    case eTargetType.Accounts:
                        notificationTarget.Type = "account_ids_target";
                        break;
                }
            }
        }

        public override string ToString()
        {
            adjustToTargetsType();

            var serializer = new JsonSerializer { NullValueHandling = NullValueHandling.Ignore };
            var notificationJObject = JObject.FromObject(this, serializer);

            if (CustomData != null)
            {
                addCustomData(notificationJObject);
            }

            return notificationJObject.ToString();
        }

        private void addCustomData(JObject notificationObject)
        {
            var notificationContentJObject = (JObject)notificationObject["notification_content"];
            var innerCustomDataJObject = (JObject)notificationContentJObject["custom_data"];


            foreach (KeyValuePair<string, string> entry in CustomData)
            {
                innerCustomDataJObject.Add(entry.Key, entry.Value);
            }
        }

        /// <summary>
        /// To serialize the object in the accepted API format - assign Targets to the matching property while setting null to the others (which disables their serialization).
        /// </summary>
        private void adjustToTargetsType()
        {
            switch (TargetType)
            {
                case eTargetType.Users:
                    notificationTarget.Users = Targets;
                    notificationTarget.Devices = notificationTarget.Accounts = null;
                    break;
                case eTargetType.Devices:
                    notificationTarget.Devices = Targets;
                    notificationTarget.Users = notificationTarget.Accounts = null;
                    break;
                case eTargetType.Accounts:
                    notificationTarget.Accounts = Targets;
                    notificationTarget.Users = notificationTarget.Devices = null;
                    break;
            }
        }

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
        public List<string> Targets { get; set; } = new List<string>();

        [JsonIgnore]
        public Dictionary<string, string> CustomData { get; set; }

        [JsonIgnore]
        public string Name
        {
            get
            {
                return NotificationContent.Name;
            }

            set
            {
                NotificationContent.Name = value;
            }
        }

        [JsonIgnore]
        public string Title
        {
            get
            {
                return NotificationContent.Title;
            }

            set
            {
                NotificationContent.Title = value;
            }
        }

        [JsonIgnore]
        public string Body
        {
            get
            {
                return NotificationContent.Body;
            }

            set
            {
                NotificationContent.Body = value;
            }
        }

        [JsonIgnore]
        protected string Icon
        {
            get
            {
                return NotificationContent.CustomData.Icon;
            }

            set
            {
                NotificationContent.CustomData.Icon = value;
            }
        }

        [JsonIgnore]
        protected string Sound
        {
            get
            {
                return NotificationContent.CustomData.Sound;
            }

            set
            {
                NotificationContent.CustomData.Sound = value;
            }
        }

        [JsonIgnore]
        protected string Color
        {
            get
            {
                return NotificationContent.CustomData.Color;
            }
            set
            {
                NotificationContent.CustomData.Color = value;
            }
        }

        #region functional properties

        private _NotificationTarget notificationTarget = new _NotificationTarget();

        [JsonProperty("notification_target")]
        private _NotificationTarget NotificationTarget
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
                    return notificationTarget;
                }
            }
        }

        [JsonProperty("notification_content")]
        private _NotificationContent NotificationContent { get; set; } = new _NotificationContent();
        #endregion

        #region private class definitions for json serialization

        private class _NotificationTarget
        {
            [JsonProperty("type")]
            public string Type { get; set; } = "user_ids_target";

            [JsonProperty("user_ids")]
            public List<string> Users { get; set; }

            [JsonProperty("devices")]
            public List<string> Devices { get; set; }

            [JsonProperty("account_ids")]
            public List<string> Accounts { get; set; }
        }

        private class _NotificationContent
        {
            [JsonProperty("name")]
            public string Name { get; set; } = string.Empty;

            [JsonProperty("title")]
            public string Title { get; set; } = string.Empty;

            [JsonProperty("body")]
            public string Body { get; set; } = string.Empty;

            [JsonProperty("custom_data")]
            public _CustomData CustomData { get; set; } = new _CustomData();
        }

        private class _CustomData
        {
            [JsonProperty("sound")]
            public string Sound { get; set; } = "default";

            [JsonProperty("icon")]
            public string Icon { get; set; }

            [JsonProperty("color")]
            public string Color { get; set; }
        }

        #endregion
    }
}