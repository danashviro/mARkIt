using Microsoft.Azure.Mobile.Server;
using System.Collections.Generic;

namespace Backend.DataObjects
{
    public class UserMarkRating : EntityData
    {
        public string MarkId { get; set; }
        public string UserEmail { get; set; }
        public int HowManyStars { get; set; }
    }
}