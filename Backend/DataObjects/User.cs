using Microsoft.Azure.Mobile.Server;
using System.Collections.Generic;

namespace Backend.DataObjects
{
    public class User : EntityData
    {
        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string Email { get; set; }

        public int RelevantCategoriesCode { get; set; }
    }
}