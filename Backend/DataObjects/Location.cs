using Microsoft.Azure.Mobile.Server;

namespace Backend.DataObjects
{
    public class Location : EntityData
    {
        public string Message { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }
    }
}