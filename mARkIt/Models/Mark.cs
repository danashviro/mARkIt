using System.Collections.Generic;
using System.Threading.Tasks;
using mARkIt.Abstractions;
using mARkIt.Services;

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

        public static async Task<List<Mark>> GetMyMarks(User i_User)
        {
            return await AzureService.MobileService.GetTable<Mark>().Where(mark => mark.UserEmail == i_User.Email).ToListAsync();       
        }

        public static async Task<List<Mark>> GetMarksAccordingToUserSettings(User i_User)
        {
            return await AzureService.MobileService.GetTable<Mark>().Where(mark => (mark.CategoriesCode & i_User.RelevantCategoriesCode) != 0).ToListAsync();
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
