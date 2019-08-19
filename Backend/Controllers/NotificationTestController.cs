using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using System.Net;

using Backend.Models;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using mARkIt.Backend.Notifications;

namespace Backend.Controllers
{
    [MobileAppController]
    public class NotificationTestController : ApiController
    {
        private const string appCenterApiToken = "2d1ebd8c1c907e453bb933b79c9d02a340247caf";

        MobileServiceContext context;

        public NotificationTestController()
        {
            context = new MobileServiceContext();
        }

        // GET api/NotificationTest
        [HttpGet]
        public async Task<string> Get()
        {
            return "test result";
        }
    }
}