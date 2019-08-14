using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using System.Net;
using System.Collections.Generic;
using Backend.DataObjects;
using System;
using System.Linq;
using Backend.Models;
using System.Windows;
using System.Security.Claims;
using mARkIt.Backend;
using mARkIt.Backend.Utils;

namespace Backend.Controllers
{
    [MobileAppController]
    public class AuthenticationTestController : ApiController
    {
        private const double k_EarthRadius = 6371e3;

        private const double k_RelevantMarksDistanceRadius = 10;


        MobileServiceContext context;

        public AuthenticationTestController()
        {
            context = new MobileServiceContext();
        }

        // GET api/RelevantMarks
        [HttpGet]
        public string Get()
        {
            string res = this.GetLoggedUserId();

            if (res == null)
            {
                res = "fail";
            }

            return res;
        }

        [HttpPost]
        public bool Post()
        {
            return true;
        }
    }
}