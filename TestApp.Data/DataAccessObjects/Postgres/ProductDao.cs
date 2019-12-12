using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestApp.Data.Interfaces;
using TestApp.Data.Models;
using Microsoft.Extensions.Logging;
using TestApp.Shared.Enumerations;

namespace TestApp.Data.DataAccessObjects.Postgres
{
    public class ProductDao : BaseDao, IProductDao
    {
        public ProductDao(string connectionString, ILogger logger, DatabaseProvider provider) : base(connectionString, logger, provider) { }

        public async Task Create(Product model)
        {
            try
            {
                _logger.LogInformation("Trying to execute sql create product query");
                model.Id = await QuerySingleOrDefaultAsync<int>($@"
                        insert into{"\"Product\""} (
                           {"\"Count\""},
                           {"\"Price\""},
                           {"\"Name\""}
                        ) values (
                            @Count,
                            @Price,
                            @Name
                        );
                        select SCOPE_IDENTITY();
                ", model);
                _logger.LogInformation("Sql create product query successfully executed");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        public async Task Delete(IReadOnlyList<int> ids)
        {
            try
            {
                _logger.LogInformation("Trying to execute sql delete products query");
                await ExecuteAsync($@"
                    delete from{"\"Product\""}
                    where{"\"Id\""} = any(@ids)
                ", new { ids });
                _logger.LogInformation("Sql delete products query successfully executed");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        public async Task<IEnumerable<Product>> Get(ProductGetOptions options)
        {
            try
            {
                StringBuilder sql = new StringBuilder();

                _logger.LogInformation("Try to create get products sql query");

                sql.AppendLine($@"
                    select 
                       {"\"Id\""},
                       {"\"Count\""},
                       {"\"Price\""},
                       {"\"Name\""}
                    from{"\"Product\""}
                ");

                int conditionIndex = 0;
                if (options.Id.HasValue)
                {
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} ({"\"Id\""} = @id)");
                }
                if (options.Ids != null)
                {
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} ({"\"Id\""} = any(@ids))");
                }
                if (!string.IsNullOrEmpty(options.NormalizedSearch))
                {
                    sql.AppendLine($@"
                        {(conditionIndex++ == 0 ? "where" : "and")} (lower({"\"Name\""}) like lower(@NormalizedSearch))
                    ");
                }
                if(!string.IsNullOrEmpty(options.Name))
                {
                    sql.AppendLine($@"{(conditionIndex++ == 0 ? "where" : "and")} ({"\"Name\""} = @Name)");
                }
                _logger.LogInformation($"Sql query successfully created:\n{sql.ToString()}");

                _logger.LogInformation("Try to execute sql get products query");
                var result = await QueryAsync<Product>(sql.ToString(), options);
                _logger.LogInformation("Sql get products query successfully executed");
                return result;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        public async Task Update(Product model)
        {
            try
            {
                _logger.LogInformation("Trying to execute sql update product query");
                await ExecuteAsync($@"
                    update{"\"Product\""} set
                       {"\"Count\""} = @Count,
                       {"\"Price\""} = @Price,
                       {"\"Name\""} = @Name
                    where{"\"Id\""} = @Id
                ", model);
                _logger.LogInformation("Sql update product query successfully executed");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }
    }
}