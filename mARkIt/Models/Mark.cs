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
        public int RatingsCount { get; }
        public float Rating { get; }

        public static async Task<List<Mark>> GetMyMarks(User i_User)
        {
            return await AzureService.MobileService.GetTable<Mark>().Where(mark => mark.UserEmail == i_User.Email).ToListAsync();       
        }

        /// <summary>
        /// Get a list of marks filtered by categories and/or location.
        /// </summary>
        /// <param name="i_RelevantCategoriesCode"> In case filteration by location only is desired - input (int)eCategories.All </param>
        /// <param name="i_Longitube"> Can be omitted if filteration by location is not desired - only if i_Latitude is ommited as well </param>
        /// <param name="i_Latitude"> Can be omitted if filteration by location is not desired - only if i_Longitude is ommited as well </param>
        /// <returns>
        /// A list of marks if the fetching operation was successful, otherwise returns null;        
        /// </returns>
        public static async Task<List<Mark>> GetRelevantMarks(int i_RelevantCategoriesCode = (int)eCategories.All, double? i_Longitube = null, double? i_Latitude = null)
        {
            List<Mark> relevantMarks;

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "relevantCategoriesCode", i_RelevantCategoriesCode.ToString() },
                { "longitude", i_Longitube.ToString() },
                { "latitude", i_Latitude.ToString()}
            };

            try
            {
                relevantMarks = await AzureService.MobileService.InvokeApiAsync<List<Mark>>("RelevantMarks", HttpMethod.Get, parameters);
            }
            catch(Exception ex)
            {
                relevantMarks = null;
            }

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
