using Microsoft.Azure.Mobile.Server;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.DataObjects
{
    public class UserMarkRating
    {
        [StringLength(450)]
        [Key, Column(Order = 0)]
        public string UserEmail { get; set; }

        [StringLength(450)]
        [ForeignKey("Mark")]
        [Key, Column(Order = 1)]
        public string MarkId { get; set; }

        public virtual Mark Mark {get; set; }

        public double Rating { get; set; }
    }
}