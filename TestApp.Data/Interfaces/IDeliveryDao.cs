using System.Collections.Generic;
using System.Threading.Tasks;
using TestApp.Data.Models;

namespace TestApp.Data.Interfaces{
    public interface IDeliveryDao
    {        
        Task<IEnumerable<Delivery>> Get(DeliveryGetOptions options);
        Task Create(Delivery model);
        Task Update(Delivery model);
        Task Delete(IReadOnlyList<int> ids);
    }
}