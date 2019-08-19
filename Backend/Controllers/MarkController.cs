using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Backend.DataObjects;
using Backend.Models;
using System;
using mARkIt.Backend;
using System.Net;
using mARkIt.Backend.Utils;
using mARkIt.Backend.Notifications;
using System.Collections.Generic;

namespace Backend.Controllers
{
    public class MarkController : TableController<Mark>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
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
            await pushNotification();
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        private async Task pushNotification()
        {
            Notification notification = new Notification
            {
                IsBroadcast = false,
                Targets = new List<string> { LoggedUserId },
                Name = $"Mark upload - {DateTime.Now.ToString("MMddHHmmss")}",
                Title = "Congrats!",
                Body = "You've uploaded a new mark!"
            };
            await notification.Push();
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
