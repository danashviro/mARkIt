using mARkIt.Abstractions;
using mARkIt.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using mARkIt.Utils;
using System.Net.Http;

namespace mARkIt.Models
{
    public class User : TableData
    {
        public User()
        { }

        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public int RelevantCategoriesCode { get; set; }

        public static async Task<bool> Insert(User i_User)
        {
            return await AzureService.Insert(i_User);
        }

        public static async Task<bool> Delete(User i_User)
        {
            return await AzureService.Delete(i_User);
        }

        public static async Task<bool> Update(User i_User)
        {
            return await AzureService.Update(i_User);
        }

        public static async Task<User> GetUserByEmail(string i_Email)
        {
            List<User> users= await AzureService.MobileService.GetTable<User>().Where(u => u.Email == i_Email).ToListAsync();
            if(users.Count==0)
            {
                User user = new User()
                {
                    Email = i_Email,
                    RelevantCategoriesCode = (int)eCategories.All
                };
                await Insert(user);
                users = await AzureService.MobileService.GetTable<User>().Where(u => u.Email == i_Email).ToListAsync();
            }
            return users.First();
        }

        /// <summary>
        /// Posts a new rating of mark by a user, or update his rating for the same mark.
        /// </summary>
        /// <param name="i_Email"></param>
        /// <param name="i_MarkId"></param>
        /// <param name="i_Rating"></param>
        /// <returns>
        /// Returns 'true' if the rating was added/updated successfully, 'false' otherwise.
        /// 
        /// Note: Also returns 'false' if the user or the mark do not exist in the database.
        /// </returns>
        public static async Task<bool> RateMark(string i_Email, string i_MarkId, float i_Rating)
        {
            bool updateSuccessful = false;

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "userEmail", i_Email },
                { "markId",i_MarkId },
                { "rating", i_Rating.ToString()}
            };

            try
            {
                updateSuccessful = await AzureService.MobileService.InvokeApiAsync<bool>("Rating", HttpMethod.Post, parameters);
            }
            catch(Exception)
            {
                updateSuccessful = false;
            }

            return updateSuccessful;
        }

        /// <summary>
        /// Get the rating an existing user gave to an existing mark.
        /// 
        /// 
        /// </summary>
        /// <param name="i_Email"></param>
        /// <param name="i_MarkId"></param>
        /// <returns>
        /// Returns a 'double?' which will hold the rating if it was fetched successfully, 'null' otherwise. 
        ///  
        /// Note: Also returns 'null' if a rating of the mark by the user does not exist in the database.
        /// </returns>
        public static async Task<float?> GetUserRatingForMark(string i_Email, string i_MarkId)
        {
            float? userRatingOfMark;

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "userEmail", i_Email },
                { "markId",i_MarkId }
            };

            try
            {
                userRatingOfMark = await AzureService.MobileService.InvokeApiAsync<float?>("Rating", HttpMethod.Get, parameters);
            }
            catch(Exception)
            {
                return userRatingOfMark = null;
            }

            return userRatingOfMark;
        }
    }
}
