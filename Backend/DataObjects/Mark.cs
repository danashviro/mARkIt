using System;
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
        public float RatingsSum { get; set; }
        public int RatingsCount { get; set; }
        public float Rating { get; set; }

        internal void UpdateRating()
        {
            Rating = RatingsSum / RatingsCount;
        }
    }
}