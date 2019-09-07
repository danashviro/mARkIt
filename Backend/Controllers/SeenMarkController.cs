using Backend.Models;
using Microsoft.Azure.Mobile.Server.Config;
using System;
using System.Web.Http;
using mARkIt.Backend.Utils;
using System.Threading.Tasks;
using System.Net;
using System.Data.Entity;
using mARkIt.Backend.DataObjects;

namespace mARkIt.Backend.Controllers
{
    [MobileAppController]
    [Authorize]
    public class SeenMarkController : ApiController
    {
        MobileServiceContext context;
        public string LoggedUserId => this.GetLoggedUserId();

        public SeenMarkController()
        {
            context = new MobileServiceContext();
        }

        // POST api/SeenMark
        [HttpPost]
        public async Task<bool> Post(string markId)
        {
            bool updateWasSuccessful = false;

            // Check parameters
            if (markId == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            // Use a transaction to update the database
            using (DbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    UserMarkExperience userMarkExperience = await context.UserMarkExperiences.FindAsync(LoggedUserId, markId);

                    if (userMarkExperience == null)
                    {
                        // Create and add the new user-mark experience
                        userMarkExperience = context.UserMarkExperiences.Create();
                        userMarkExperience.UserId = LoggedUserId;
                        userMarkExperience.MarkId = markId;

                        context.UserMarkExperiences.Add(userMarkExperience);
                    }

                    else
                    {
                        validateOwner(userMarkExperience);
                    }

                    userMarkExperience.LastSeen = DateTime.Now;

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

        public void validateOwner(UserMarkExperience userMarkRating)
        {
            if (userMarkRating.UserId != LoggedUserId)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}