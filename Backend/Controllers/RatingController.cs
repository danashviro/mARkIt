using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using System.Net;
using Backend.DataObjects;
using System;
using Backend.Models;
using System.Data.Entity;
using System.Threading.Tasks;
using mARkIt.Backend;
using mARkIt.Backend.Utils;

namespace Backend.Controllers
{
    [MobileAppController]
    public class RatingController : ApiController
    {
        MobileServiceContext context;
        public string LoggedUserId => this.GetLoggedUserId();

        public RatingController()
        {
            context = new MobileServiceContext();
        }

        // GET api/RatingController
        [HttpGet]
        public double? Get(string markId)
        {
            // Check parameters validity
            if (markId == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            UserMarkRating userMarkRating = context.UserMarkRatings.Find(LoggedUserId, markId);

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
        public async Task<bool> Post(string markId, float? rating)
        {
            bool updateWasSuccessful = false;

            // Check parameters
            if (markId == null || rating == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            // Use a transaction to update the database
            using (DbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    UserMarkRating userMarkRating = await context.UserMarkRatings.FindAsync(LoggedUserId, markId);

                    if (userMarkRating == null)
                    {
                        // Create and add the new rating
                        userMarkRating = context.UserMarkRatings.Create();
                        userMarkRating.UserId = LoggedUserId;
                        userMarkRating.MarkId = markId;
                        userMarkRating.Rating = rating.Value;

                        context.UserMarkRatings.Add(userMarkRating);

                        // Update the mark
                        userMarkRating.Mark.RatingsCount++;
                        userMarkRating.Mark.RatingsSum += rating.Value;
                    }

                    else
                    {
                        // Update the existing rating of the mark
                        validateOwner(userMarkRating);
                        userMarkRating.Mark.RatingsSum -= userMarkRating.Rating;
                        userMarkRating.Mark.RatingsSum += rating.Value;
                        userMarkRating.Rating = rating.Value; // Update the value at the UserMarkRating entry
                    }

                    userMarkRating.Mark.UpdateRating();
                    await context.SaveChangesAsync();
                    transaction.Commit();
                    updateWasSuccessful = true;
                }

                catch (Exception ex)
                {
                    LogTools.LogException(ex);
                    transaction.Rollback();
                    throw ex;
                }
            }

            return updateWasSuccessful;
        }

        public void validateOwner(UserMarkRating userMarkRating)
        {
            if (userMarkRating.UserId != LoggedUserId)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}