using mARkIt.Abstractions;

namespace mARkIt.Models
{    public class UserMarkRating : TableData
    {
        public string MarkId { get; set; }
        public string UserEmail { get; set; }
        public int HowManyStars { get; set; }
    }
}
