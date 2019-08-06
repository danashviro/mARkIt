using mARkIt.Abstractions;

namespace mARkIt.Models
{
    public class User : TableData
    {
        public User()
        { }

        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public int RelevantCategoriesCode { get; set; }
    }
}
