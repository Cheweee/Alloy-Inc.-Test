using System.Collections.Generic;
using System.Threading.Tasks;
using TestApp.Data.Models;

namespace TestApp.Data.Interfaces{
    public interface IProductDao
    {        
        Task<IEnumerable<Product>> Get(ProductGetOptions options);
        Task Create(Product model);
        Task Update(Product model);
        Task Delete(IReadOnlyList<int> ids);
    }
}