using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mARkIt.Backend.Notifications
{
    public enum eTargetType
    {
        /// <summary>
        /// User identity set through the AppCenter SDK.
        /// Set at the client through: AppCenter.SetUserId(userId).
        /// </summary>
        Users,

        /// <summary> 
        /// The App Center SDK creates a unique Install ID (UUID) for each device once the app is installed.
        /// Obtained by calling AppCenter.GetInstallIdAsync() from the client.
        /// </summary>
        Devices,

        /// <summary> User identity set using AppCenter Auth. </summary>
        Accounts
    }
}