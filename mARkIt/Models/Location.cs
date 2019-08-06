using mARkIt.Abstractions;


namespace mARkIt.Models
{
    public class Location : TableData
    {
        public Location()
        {}

        public double latitude { get; set; }
        public double longitude { get; set; }
        public string message { get; set; }        
    }
}
