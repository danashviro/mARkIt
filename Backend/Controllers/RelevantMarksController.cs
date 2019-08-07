using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using System.Net;
using System.Collections.Generic;
using Backend.DataObjects;
using System;
using System.Linq;
using Backend.Models;
using System.Windows;

namespace Backend.Controllers
{
    [MobileAppController]
    public class RelevantMarksController : ApiController
    {
        MobileServiceContext context;

        public RelevantMarksController()
        {
            context = new MobileServiceContext();
        }

        // GET api/RelevantMarks
        public List<Mark> Get(int? relevantCategoriesCode, double? longitude, double? latitude)
        {
            if (relevantCategoriesCode == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }            

            var relevantMarks = from mark in context.Marks
                                where (relevantCategoriesCode & mark.CategoriesCode) != 0
                                select mark;

            if(longitude.HasValue && latitude.HasValue)
            {
                relevantMarks = from mark in relevantMarks
                                where closeEnough(new Vector(mark.Longitude, mark.Latitude), new Vector(longitude.Value, latitude.Value))
                                select mark;
            }

            return relevantMarks.ToList();
        }

        private bool closeEnough(Vector userPos, Vector markPos)
        {
            throw new NotImplementedException();
        }
    }
}