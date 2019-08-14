using Microsoft.Azure.Mobile.Server;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.DataObjects
{
    public class UserMarkRating
    {
        [StringLength(450)]
        [ForeignKey("User")]
        [Key, Column(Order = 0)]
        public string UserId { get; set; }

        public virtual User User { get; set; }


        [StringLength(450)]
        [ForeignKey("Mark")]
        [Key, Column(Order = 1)]
        public string MarkId { get; set; }

        public virtual Mark Mark {get; set; }

        public float Rating { get; set; }
    }
}