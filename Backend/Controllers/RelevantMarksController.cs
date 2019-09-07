using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using System.Collections.Generic;
using System;
using System.Linq;
using Backend.Models;
using System.Windows;
using mARkIt.Backend.Utils;
using mARkIt.Backend.DataObjects;

namespace Backend.Controllers
{
    [MobileAppController]
    [Authorize]
    public class RelevantMarksController : ApiController
    {
        private const double k_EarthRadius = 6371e3;

        private const double k_DefaultProximityThreshhold = 10;

        MobileServiceContext context;

        public string LoggedUserId => this.GetLoggedUserId();

        public RelevantMarksController()
        {
            context = new MobileServiceContext();
        }

        // GET api/RelevantMarks
        public List<Mark> Get(double? longitude, double? latitude, double? proximityThreshhold)
        {
            List<Mark> relevantMarksByCategoryAndProximity = null;

            int relevantCategoriesCode = getUserRelevantCateogiresCode();

            var relevantMarksByCategory =  from mark in context.Marks
                                           where (relevantCategoriesCode & mark.CategoriesCode) != 0
                                           select mark;

            if (longitude.HasValue && latitude.HasValue)
            {
                if (!proximityThreshhold.HasValue)
                {
                    proximityThreshhold = k_DefaultProximityThreshhold;
                }

                relevantMarksByCategoryAndProximity = new List<Mark>();
                foreach (Mark mark in relevantMarksByCategory)
                {
                    Vector userPos = new Vector(longitude.Value, latitude.Value);
                    Vector markPos = new Vector(mark.Longitude, mark.Latitude);
                    if (markIsCloseEnough(userPos, markPos, proximityThreshhold.Value))
                    {
                        relevantMarksByCategoryAndProximity.Add(mark);
                    }
                }
            }
            else
            {
                relevantMarksByCategoryAndProximity = relevantMarksByCategory.ToList();
            }

            return relevantMarksByCategoryAndProximity;
        }

        private int getUserRelevantCateogiresCode()
        {
            User user = context.Users.Find(LoggedUserId);
            return user.RelevantCategoriesCode;

            /// var userQuery = from user in context.Users where user.Id == LoggedUserId select user.RelevantCategoriesCode;
            /// return userQuery.First();
        }

        private bool markIsCloseEnough(Vector userPos, Vector markPos, double proximityThreshhold)
        {
            return distanceInKm(userPos, markPos) < proximityThreshhold;
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

        private static double toRadians(double degree)
        {
            return degree * Math.PI / 180;
        }
    }
}