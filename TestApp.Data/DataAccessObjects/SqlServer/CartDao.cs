using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestApp.Data.Interfaces;
using TestApp.Data.Models;
using Microsoft.Extensions.Logging;

namespace TestApp.Data.DataAccessObjects.SqlServer
{
    public class CartDao : BaseDao, ICartDao
    {
        public CartDao(string connectionString, ILogger logger) : base(connectionString, logger) { }

        public async Task Create(Cart model)
        {
            try
            {
                _logger.LogInformation("Trying to execute sql create cart query");
                await ExecuteAsync(@"
                        insert into Cart (
                            Count,
                            Price,
                            OrderId,
                            ProductId,
                            Name
                        ) values (
                            @Count,
                            @Price,
                            @OrderId,
                            @ProductId,
                            @Name
                        );
                ", model);
                _logger.LogInformation("Sql create cart query successfully executed");
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
                _logger.LogInformation("Trying to execute sql delete carts query");
                await ExecuteAsync(@"
                    delete from Cart
                    where Id in @ids
                ", new { ids });
                _logger.LogInformation("Sql delete carts query successfully executed");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        public async Task<IEnumerable<Cart>> Get(CartGetOptions options)
        {
            try
            {
                StringBuilder sql = new StringBuilder();

                _logger.LogInformation("Try to create get carts sql query");

                sql.AppendLine(@"
                    select 
                        Id,
                        OrderId,
                        ProductId,
                        Count,
                        Price,
                        Name
                    from Cart
                ");

                int conditionIndex = 0;
                if (options.Id.HasValue)
                {
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} (Id = @id)");
                }
                if (options.ProductId.HasValue)
                {
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} (ProductId = @ProductId)");
                }
                if (options.OrderId.HasValue)
                {
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} (OrderId = @OrderId)");
                }
                if (options.Ordered.HasValue)
                {
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} (OrderId is {(options.Ordered.Value ? "not" : string.Empty)} null)");
                }
                if (options.Ids != null)
                {
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} (Id in @ids)");
                }
                if(options.OrderIds != null)
                {
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} (OrderId in @OrderIds)");
                }
                if (!string.IsNullOrEmpty(options.NormalizedSearch))
                {
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} (lower(Name) like lower(@NormalizedSearch))");
                }
                if (!string.IsNullOrEmpty(options.Name))
                {
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} (Name = @Name)");
                }
                _logger.LogInformation($"Sql query successfully created:\n{sql.ToString()}");

                _logger.LogInformation("Try to execute sql get carts query");
                var result = await QueryAsync<Cart>(sql.ToString(), options);
                _logger.LogInformation("Sql get carts query successfully executed");
                return result;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        public async Task Update(Cart model)
        {
            try
            {
                _logger.LogInformation("Trying to execute sql update cart query");
                await ExecuteAsync(@"
                    update Cart set
                        ProductId = @ProductId,
                        OrderId = @OrderId,
                        Count = @Count,
                        Price = @Price,
                        Name = @Name
                    where Id = @Id
                ", model);
                _logger.LogInformation("Sql update cart query successfully executed");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }
    }
}