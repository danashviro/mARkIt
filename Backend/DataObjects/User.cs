using Microsoft.Azure.Mobile.Server;
using System.Collections.Generic;

namespace Backend.DataObjects
{
    public class User : EntityData
    {
        public int RelevantCategoriesCode { get; set; }
    }
}