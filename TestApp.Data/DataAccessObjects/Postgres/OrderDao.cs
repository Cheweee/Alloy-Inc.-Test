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
    public class OrderDao : BaseDao, IOrderDao
    {
        public OrderDao(string connectionString, ILogger logger, DatabaseProvider provider) : base(connectionString, logger, provider) { }

        public async Task Create(Order model)
        {
            try
            {
                _logger.LogInformation("Trying to execute sql create order query");
                model.Id = await QuerySingleOrDefaultAsync<int>($@"
                        insert into {"\"Order\""} (
                           {"\"DeliveryId\""},
                           {"\"FullName\""},
                           {"\"Addres\""},
                           {"\"SpecialDate\""},
                           {"\"PaymentMethod\""}
                        ) values (
                            @DeliveryId,
                            @FullName,
                            @Addres,
                            @SpecialDate,
                            @PaymentMethod
                        );
                        returning{"\"Id\""};
                ", model);
                _logger.LogInformation("Sql create order query successfully executed");
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
                _logger.LogInformation("Trying to execute sql delete orders query");
                await ExecuteAsync($@"
                    delete from {"\"Order\""}
                    where{"\"Id\""} in @ids
                ", new { ids });
                _logger.LogInformation("Sql delete orders query successfully executed");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        public async Task<IEnumerable<Order>> Get(OrderGetOptions options)
        {
            try
            {
                StringBuilder sql = new StringBuilder();

                _logger.LogInformation("Try to create get orders sql query");

                sql.AppendLine($@"
                    select 
                       {"\"Id\""},
                       {"\"DeliveryId\""},
                       {"\"FullName\""},
                       {"\"Addres\""},
                       {"\"SpecialDate\""},
                       {"\"PaymentMethod\""}
                    from {"\"Order\""}
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
                        {(conditionIndex++ == 0 ? "where" : "and")} (lower({"\"FullName\""}) like lower(@NormalizedSearch))
                    ");
                }
                _logger.LogInformation($"Sql query successfully created:\n{sql.ToString()}");

                _logger.LogInformation("Try to execute sql get orders query");
                var result = await QueryAsync<Order>(sql.ToString(), options);
                _logger.LogInformation("Sql get orders query successfully executed");
                return result;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        public async Task Update(Order model)
        {
            try
            {
                _logger.LogInformation("Trying to execute sql update order query");
                await ExecuteAsync($@"
                    update {"\"Order\""} set
                       {"\"DeliveryId\""} = @DeliveryId,
                       {"\"FullName\""} = @FullName,
                       {"\"Addres\""} = @Addres,
                       {"\"SpecialDate\""} = @SpecialDate,
                       {"\"PaymentMethod\""} = @PaymentMethod
                    where{"\"Id\""} = @Id
                ", model);
                _logger.LogInformation("Sql update order query successfully executed");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }
    }
}