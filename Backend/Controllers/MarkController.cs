using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Backend.Models;
using System;
using System.Net;
using mARkIt.Backend.Utils;
using mARkIt.Backend.Notifications;
using System.Collections.Generic;
using mARkIt.Backend.DataObjects;

namespace Backend.Controllers
{
    public class MarkController : TableController<Mark>
    {
        MobileServiceContext context;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Mark>(context, Request);
        }

        public string LoggedUserId => this.GetLoggedUserId();

        // GET tables/Mark
        public IQueryable<Mark> GetAllMark()
        {
            return Query();
        }

        // GET tables/Mark/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Mark> GetMark(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Mark/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Authorize]
        public Task<Mark> PatchMark(string id, Delta<Mark> patch)
        {
            validateOwner(id);
            return UpdateAsync(id, patch);
        }

        // POST tables/Mark
        [Authorize]
        public async Task<IHttpActionResult> PostMark(Mark item)
        {
            item.UserId = LoggedUserId;
            Mark current = await InsertAsync(item);

            string userPushId = context.Users.Find(LoggedUserId).NotificationsId;

            if(!string.IsNullOrEmpty(userPushId))
            {
                Notification notification = new MarkitNotification
                {
                    Targets = new List<string> { userPushId },
                    TargetType = eTargetType.Devices,
                    Name = $"Mark upload - {DateTime.Now.ToString("MM.dd HH:mm:ss.fff")}",
                    Title = "Congrats!",
                    Body = "You've uploaded a new mark!"
                };

                await notification.Push();
            }

            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Mark/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Authorize]
        public Task DeleteMark(string id)
        {
            validateOwner(id);
            return DeleteAsync(id);
        }

        public void validateOwner(string id)
        {
            var result = Lookup(id).Queryable.Where(item => item.UserId.Equals(LoggedUserId)).FirstOrDefault<Mark>();
            if (result == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}
