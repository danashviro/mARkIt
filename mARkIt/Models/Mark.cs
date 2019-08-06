using mARkIt.Abstractions;

namespace mARkIt.Models
{
    class Mark : TableData
    {
        public string UserEmail { get; set; }
        public string Message { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Style { get; set; }
        public int CategoriesCode { get; set; }
    }
}
