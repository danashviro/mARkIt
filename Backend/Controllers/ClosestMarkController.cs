﻿using System.Web.Http;
using mARkIt.Backend.DataObjects;
using Microsoft.Azure.Mobile.Server.Config;
using mARkIt.Backend.Utils;
using System.Linq;
using System.Collections.Generic;
using Backend.Models;
using System;
using System.Windows;
using System.Threading.Tasks;

namespace mARkIt.Backend.Controllers
{
    [MobileAppController]
    [Authorize]
    public class ClosestMarkController : ApiController
    {
        private string LoggedUserId => this.GetLoggedUserId();        

        MobileServiceContext context;

        public ClosestMarkController()
        {
            context = new MobileServiceContext();
        }

        private double m_UserLatitude;
        private double m_UserLongitude;

        // GET api/ClosestMark
        public async Task<Mark> Get(double? latitude, double? longitude)
        {
            Mark closestMark = null;

            if (latitude != null && longitude != null)
            {
                m_UserLatitude = latitude.Value;
                m_UserLongitude = longitude.Value;
            }

            try
            {
                IEnumerable<string> seenMarksIds = from userMarkExperiences in context.UserMarkExperiences
                                                   where userMarkExperiences.UserId == LoggedUserId
                                                   select userMarkExperiences.MarkId;
                               
                IEnumerable<Mark> unseenMarks = from marks in context.Marks
                                                where !seenMarksIds.Contains(marks.Id)
                                                select marks;

                if (unseenMarks.Count() != 0)
                {
                    closestMark = unseenMarks.OrderByDescending((mark) => distanceFromUserKm(mark)).Last();

                    double distanceFromClosestMark = distanceFromUserKm(closestMark);
                    if (distanceFromClosestMark > 0.1)
                    {
                        // Closest mark is too far
                        closestMark = null;
                    }
                    else
                    {
                        // Insert a UserMarkExperience to avoid notifying this user about this mark again
                        await context.InsertUserMarkExperience(LoggedUserId, closestMark.Id);
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