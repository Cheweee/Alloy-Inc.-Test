using System.Collections.Generic;
using System.Threading.Tasks;
using TestApp.Data.Models;

namespace TestApp.Data.Interfaces
{
    public interface IUserLocationDao
    {        
        Task<IEnumerable<UserLocation>> Get(UserLocationGetOptions options);
        Task Create(IEnumerable<UserLocation> model);
        Task Update(IEnumerable<UserLocation> model);
        Task Delete(IReadOnlyList<int> ids);
    }
}