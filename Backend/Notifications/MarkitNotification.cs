using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mARkIt.Backend.Notifications
{
    public class MarkitNotification : Notification
    {
        private const string k_AppCenterApiToken = "2d1ebd8c1c907e453bb933b79c9d02a340247caf";
        private const string k_AppCenterOrganizationName = "MTA_FinalProject_mARk-It";
        private const string k_AppCenterAndroidProjectName = "mARk-It.Android";
        private const string k_AppCenterIOSProjectName = "mARk-It.iOS";

        private NotificationJson m_AndroidNotificationObj;
        private NotificationJson m_iOSNotificationObj;
        protected override NotificationJson AndroidNotificationObj => m_AndroidNotificationObj;
        protected override NotificationJson iOSNotificationObj => m_iOSNotificationObj;

        public MarkitNotification() : base(
            k_AppCenterApiToken,
            k_AppCenterOrganizationName,
            k_AppCenterAndroidProjectName,
            k_AppCenterIOSProjectName)
        {
            m_AndroidNotificationObj = new NotificationJson();
            m_AndroidNotificationObj.CustomData["icon"] = "drawable/mainlogo";
            m_AndroidNotificationObj.CustomData["sound"] = "default";

            m_iOSNotificationObj = new NotificationJson();
            m_iOSNotificationObj.CustomData["icon"] = "icons/appicon_240_240";
            m_iOSNotificationObj.CustomData["sound"] = "default";

            IsBroadcast = false;
            TargetType = eTargetType.Users;            
        }
    }
}