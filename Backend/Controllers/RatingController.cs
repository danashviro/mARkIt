using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using System.Net;
using System.Collections.Generic;
using Backend.DataObjects;
using System;
using System.Linq;
using Backend.Models;
using System.Windows;
using System.Data.Entity;
using System.Net.Http;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using mARkIt.Backend;

namespace Backend.Controllers
{
    [MobileAppController]
    public class RatingController : ApiController
    {
        private const double k_EarthRadius = 6371e3;

        private const double k_RelevantMarksDistanceRadius = 10;


        MobileServiceContext context;

        public RatingController()
        {
            context = new MobileServiceContext();
        }

        // GET api/RatingController
        [HttpGet]
        public double? Get(string userEmail, string markId)
        {
            // Check parameters validity
            if (userEmail == null || markId == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            UserMarkRating userMarkRating = context.UserMarkRatings.Find(userEmail, markId);

            if (userMarkRating == null)
            {
                return null;
            }
            else
            {
                return userMarkRating.Rating;
            }
        }

        [HttpPost]
        public async Task<bool> Post(string userEmail, string markId, double? rating)
        {
            bool updateWasSuccessful = false;

            // Check parameters
            if (userEmail == null || markId == null || rating == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            Mark mark = await context.Marks.FindAsync(markId);
            bool userExists = checkUserExists(userEmail);

            if (userExists && mark != null)
            {
                // Use a transaction to update the database
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        UserMarkRating userMarkRating = await context.UserMarkRatings.FindAsync(userEmail, markId);

                        if (userMarkRating == null)
                        {
                            // Update the mark
                            mark.RatingsCount++;
                            mark.RatingsSum += rating.Value;

                            // Add the new rating
                            UserMarkRating newRating = context.UserMarkRatings.Create();
                            newRating.UserEmail = userEmail;
                            newRating.MarkId = markId;
                            newRating.Rating = rating.Value;

                            context.UserMarkRatings.Add(newRating);
                        }

                        else
                        {
                            // Update the existing rating of the mark
                            mark.RatingsSum -= userMarkRating.Rating;
                            mark.RatingsSum += rating.Value;
                            userMarkRating.Rating = rating.Value; // Update the value at the UserMarkRating entry
                        }

                        await context.SaveChangesAsync();
                        transaction.Commit();
                        updateWasSuccessful = true;
                    }

                    catch (Exception ex)
                    {
                        Utils.LogException(ex);
                        transaction.Rollback();
                        throw (ex);
                    }
                }
            }

            return updateWasSuccessful;
        }

        private bool checkUserExists(string userEmail)
        {
            var userQuery = from user in context.Users
                            where user.Email == userEmail
                            select user;

            return userQuery.Count() != 0;
        }     
    }
}