using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace mARkIt.Backend.Utils
{
    public static class Extensions
    {
        public static string GetLoggedUserId(this ApiController apiController)
        {
            string userId = null;

            var claim = ((ClaimsPrincipal)apiController.User).FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                userId = claim.Value;

                // Azure places "sid:" at the start of the generated id, which we want to remove to avoid redundancy and URI errors (because of ':' character)
                userId = userId.Replace("sid:", "");
            }

            return userId;
        }
    }


}