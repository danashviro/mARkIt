using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Backend.DataObjects;
using Backend.Models;

namespace Backend.Controllers
{
    public class UserMarkRatingController : TableController<UserMarkRating>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<UserMarkRating>(context, Request);
        }

        // GET tables/UserMarkRating
        public IQueryable<UserMarkRating> GetAllUserMarkRating()
        {
            return Query();
        }

        // GET tables/UserMarkRating/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<UserMarkRating> GetUserMarkRating(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/UserMarkRating/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<UserMarkRating> PatchUserMarkRating(string id, Delta<UserMarkRating> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/UserMarkRating
        public async Task<IHttpActionResult> PostUserMarkRating(UserMarkRating item)
        {
            UserMarkRating current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/UserMarkRating/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteUserMarkRating(string id)
        {
            return DeleteAsync(id);
        }
    }
}
