using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mARkIt.Backend.Notifications
{
    public class IOSNotificationJson : NotificationJson
    {
        public IOSNotificationJson()
        {
            Icon = "icons/appicon_240_240";
        }
    }
}