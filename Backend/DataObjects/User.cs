using Microsoft.Azure.Mobile.Server;
using System.Collections.Generic;

namespace mARkIt.Backend.DataObjects
{
    public class User : EntityData
    {
        public int RelevantCategoriesCode { get; set; }
        public string NotificationsId { get; set; }
    }
}