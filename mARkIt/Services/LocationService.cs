using mARkIt.Models;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mARkIt.Services
{
    public class LocationService
    {
        private const string BackendURL = "http://mark-api.azurewebsites.net/";
        /// private const string BackendURL = "https://26f2420c-279b-4520-8edb-65ab5d70c414.azurewebsites.net/";

        private MobileServiceClient m_MobileServiceClient;

        private LocationService()
        {
            m_MobileServiceClient = new MobileServiceClient(BackendURL);
        }

        private static LocationService sr_Instance;

        public static LocationService Instance()
        {
            if (sr_Instance == null)
            {
                sr_Instance = new LocationService();
            }
            return sr_Instance;
        }

        public async Task<IEnumerable<Location>> GetLocations()
        {
            var table = m_MobileServiceClient.GetTable<Location>();
            return await table.ReadAsync();
        }

        public async Task AddLocation(Location i_Location)
        {
            var table = m_MobileServiceClient.GetTable<Location>();
            await table.InsertAsync(i_Location);
        }
    }
}
