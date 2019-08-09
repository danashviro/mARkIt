using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using mARkIt.Abstractions;
using mARkIt.Services;
using mARkIt.Utils;

namespace mARkIt.Models
{
    public class Mark : TableData
    {
        public string UserEmail { get; set; }
        public string Message { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Style { get; set; }
        public int CategoriesCode { get; set; }
        public double RatingsSum { get; set; }
        public int RatingsCount { get; set; }
        public double Rating
        {
            get
            {
                return Math.Round(RatingsSum / RatingsCount, 2);
            }
        }

        public static async Task<List<Mark>> GetMyMarks(User i_User)
        {
            return await AzureService.MobileService.GetTable<Mark>().Where(mark => mark.UserEmail == i_User.Email).ToListAsync();       
        }

        public static async Task<List<Mark>> GetRelevantMarks(int i_RelevantCategoriesCode = (int)eCategories.All, double? i_Longitube = null, double? i_Latitude = null)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "relevantCategoriesCode", i_RelevantCategoriesCode.ToString() },
                { "longitude", i_Longitube.ToString() },
                { "latitude", i_Latitude.ToString()}
            };

            List<Mark> relevantMarks = await AzureService.MobileService.InvokeApiAsync<List<Mark>>("RelevantMarks", HttpMethod.Get, parameters);
            return relevantMarks;
        }

        public static async Task<bool> Insert(Mark i_Mark)
        {
            return await AzureService.Insert(i_Mark);
        }

        public static async Task<bool> Delete(Mark i_Mark)
        {
            return await AzureService.Delete(i_Mark);
        }

        public static async Task<bool> Update(Mark i_Mark)
        {
            return await AzureService.Update(i_Mark);
        }

    }
}
