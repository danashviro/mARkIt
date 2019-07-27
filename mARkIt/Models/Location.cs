using System;


namespace mARkIt.Models
{
    public class Location
    {
        public Location()
        {
        }
        public bool deleted { get; set; }
        public DateTime updatedAt { get; set; }
        public DateTime createdAt { get; set; }
        public string version { get; set; }
        public string id { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string message { get; set; }
        
    }
}
