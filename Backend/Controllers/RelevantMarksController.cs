using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using System.Net;
using System.Collections.Generic;
using Backend.DataObjects;
using System;
using System.Linq;
using Backend.Models;
using System.Windows;

namespace Backend.Controllers
{
    [MobileAppController]
    public class RelevantMarksController : ApiController
    {
        private const double k_EarthRadius = 6371e3;

        private const double k_RelevantMarksDistanceRadius = 10;


        MobileServiceContext context;

        public RelevantMarksController()
        {
            context = new MobileServiceContext();
        }

        // GET api/RelevantMarks
        public List<Mark> Get(int? relevantCategoriesCode, double? longitude, double? latitude)
        {
            if (relevantCategoriesCode == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }            

            var relevantMarks = from mark in context.Marks
                                where (relevantCategoriesCode & mark.CategoriesCode) != 0
                                select mark;

            if(longitude.HasValue && latitude.HasValue)
            {
                relevantMarks = from mark in relevantMarks
                                where markIsCloseEnough(new Vector(mark.Longitude, mark.Latitude), new Vector(longitude.Value, latitude.Value))
                                select mark;
            }

            return relevantMarks.ToList();
        }

        private bool markIsCloseEnough(Vector userPos, Vector markPos)
        {
            return distanceInKm(userPos, markPos) < k_RelevantMarksDistanceRadius;
        }

        // Derived from the article https://www.movable-type.co.uk/scripts/latlong.html
        private static double distanceInKm(Vector pos1, Vector pos2)
        {
            double lat1Rad = toRadians(pos1.X);
            double lat2Rad = toRadians(pos2.X);
            double latDeltaRad = toRadians(pos1.X - pos2.X);
            double lonDeltaRad = toRadians(pos1.Y - pos2.Y);

            double calculatedValue1 = Math.Sin(latDeltaRad / 2) * Math.Sin(latDeltaRad / 2) +
                    Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                    Math.Sin(lonDeltaRad / 2) * Math.Sin(lonDeltaRad / 2);
            double calculatedValue2 = 2 * Math.Atan2(Math.Sqrt(calculatedValue1), Math.Sqrt(1 - calculatedValue1));

            //Based on earth radius, in km
            double distance = (k_EarthRadius * calculatedValue2) / 1000; 

            return distance;
        }

        private static double toRadians(double i_Degree)
        {
            return i_Degree * Math.PI / 180;
        }
    }
}