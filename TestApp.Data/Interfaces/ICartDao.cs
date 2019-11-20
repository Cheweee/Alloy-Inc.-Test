using System.Collections.Generic;
using System.Threading.Tasks;
using TestApp.Data.Models;

namespace TestApp.Data.Interfaces{
    public interface ICartDao
    {        
        Task<IEnumerable<Cart>> Get(CartGetOptions options);
        Task Create(Cart model);
        Task Update(Cart model);
        Task Delete(IReadOnlyList<int> ids);
    }
}