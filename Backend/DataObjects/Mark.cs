using Microsoft.Azure.Mobile.Server;

namespace Backend.DataObjects
{
    public class Mark : EntityData
    {
        public string UserEmail { get; set; }
        public string Message { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Style { get; set; }
        public int CategoriesCode { get; set; }
        public double RatingsSum { get; set; }
        public double RatingsCount { get; set; }
        public double Rating { get; set; }

        public void UpdateRating()
        {
            Rating = RatingsSum / RatingsCount;
        }
    }
}