using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using System;
using Backend.Models;
using mARkIt.Backend.Utils;
using System.Threading.Tasks;
using System.Net;
using System.Data.Entity;
using mARkIt.Backend.DataObjects;

namespace Backend.Controllers
{
    [MobileAppController]
    [Authorize]
    public class NotificationsIdController : ApiController
    {
        MobileServiceContext context;
        private string LoggedUserId => this.GetLoggedUserId();

        public NotificationsIdController()
        {
            context = new MobileServiceContext();
        }

        // POST api/NotificationsId
        [HttpPost]
        public async Task Post(string newPushId)
        {
            if (newPushId == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            User user = await context.Users.FindAsync(LoggedUserId);
            string oldPushId = user.NotificationsId;

            if (oldPushId != newPushId)
            {
                await updateLoggedUserPushId(newPushId);
            }
        }

        // DELETE api/NotificationsId
        [HttpDelete]
        public async Task Delete()
        {
            await updateLoggedUserPushId(string.Empty);
        }

        private async Task updateLoggedUserPushId(string newPushId)
        {
            using (DbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    User user = await context.Users.FindAsync(LoggedUserId);
                    user.NotificationsId = newPushId;
                    await context.SaveChangesAsync();
                    transaction.Commit();
                }

                catch (Exception ex)
                {
                    LogTools.LogException(ex);
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
    }
}