using System.Collections.Generic;
using System.Threading.Tasks;
using TestApp.Data.Models;

namespace TestApp.Data.Interfaces{
    public interface IOrderDao
    {        
        Task<IEnumerable<Order>> Get(OrderGetOptions options);
        Task Create(Order model);
        Task Update(Order model);
        Task Delete(IReadOnlyList<int> ids);
    }
}