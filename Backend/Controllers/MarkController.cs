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
    public class MarkController : TableController<Mark>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Mark>(context, Request);
        }

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
        public Task<Mark> PatchMark(string id, Delta<Mark> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Mark
        public async Task<IHttpActionResult> PostMark(Mark item)
        {
            Mark current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Mark/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteMark(string id)
        {
            return DeleteAsync(id);
        }
    }
}
