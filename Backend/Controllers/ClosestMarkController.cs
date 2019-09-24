using System.Web.Http;
using mARkIt.Backend.DataObjects;
using Microsoft.Azure.Mobile.Server.Config;
using mARkIt.Backend.Utils;
using System.Linq;
using System.Collections.Generic;
using Backend.Models;
using System;
using System.Windows;

namespace mARkIt.Backend.Controllers
{
    [MobileAppController]
    [Authorize]
    public class ClosestMarkController : ApiController
    {
        public string LoggedUserId => this.GetLoggedUserId();

        MobileServiceContext context;

        public ClosestMarkController()
        {
            context = new MobileServiceContext();
        }

        private double m_UserLatitude;
        private double m_UserLongitude;

        // GET api/ClosestMark
        public Mark Get(double? latitude, double? longitude)
        {
            Mark closestMark = null;

            LogTools.Log("Starting ClosestMark.Get");

            if (latitude != null && longitude != null)
            {
                m_UserLatitude = latitude.Value;
                m_UserLongitude = longitude.Value;
            }

            try
            {
                LogTools.Log("getting seenMarksIds");

                IEnumerable<string> seenMarksIds = from userMarkExperiences in context.UserMarkExperiences
                                                   where userMarkExperiences.UserId == LoggedUserId
                                                   select userMarkExperiences.MarkId;


                LogTools.Log("getting unseenMarks");

                IEnumerable<Mark> unseenMarks = from marks in context.Marks
                                                where !seenMarksIds.Contains(marks.Id)
                                                select marks;

                LogTools.Log("Calculating closestsMark");

                if (unseenMarks.Count() != 0)
                {
                    LogTools.Log("Unseen marks have been found");

                    closestMark = unseenMarks.OrderByDescending((mark) => distanceFromUserKm(mark)).Last();

                    double distanceFromClosestMark = distanceFromUserKm(closestMark);
                    if (distanceFromClosestMark > 0.1)
                    {
                        LogTools.Log("Closest mark is too far, returning null");
                        LogTools.Log($"Distance from closest mark: {distanceFromClosestMark}");
                        closestMark = null;
                    }
                }
            }

            catch (Exception e)
            {
                LogTools.LogException(e);
            }

            return closestMark;
        }

        private double distanceFromUserKm(Mark mark)
        {
            Vector userPos = new Vector(m_UserLongitude, m_UserLatitude);
            Vector markPos = new Vector(mark.Longitude, mark.Latitude);
            return DistanceCalculator.DistanceInKm(userPos, markPos);
        }
    }
}