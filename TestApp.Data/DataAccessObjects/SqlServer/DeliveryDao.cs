using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestApp.Data.Interfaces;
using TestApp.Data.Models;
using Microsoft.Extensions.Logging;

namespace TestApp.Data.DataAccessObjects.SqlServer
{
    public class DeliveryDao : BaseDao, IDeliveryDao
    {
        public DeliveryDao(string connectionString, ILogger logger) : base(connectionString, logger) { }

        public async Task Create(Delivery model)
        {
            try
            {
                _logger.LogInformation("Trying to execute sql create delivery query");
                model.Id = await QuerySingleOrDefaultAsync<int>(@"
                        insert into Delivery (
                            DeliveryPrice,
                            Name
                        ) values (
                            @DeliveryPrice,
                            @Name
                        );
                        select SCOPE_IDENTITY();
                ", model);
                _logger.LogInformation("Sql create delivery query successfully executed");
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
                _logger.LogInformation("Trying to execute sql delete deliverys query");
                await ExecuteAsync(@"
                    delete from Delivery
                    where Id in @ids
                ", new { ids });
                _logger.LogInformation("Sql delete deliverys query successfully executed");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        public async Task<IEnumerable<Delivery>> Get(DeliveryGetOptions options)
        {
            try
            {
                StringBuilder sql = new StringBuilder();

                _logger.LogInformation("Try to create get deliverys sql query");

                sql.AppendLine(@"
                    select 
                        Id, DeliveryPrice, Name
                    from Delivery
                ");

                int conditionIndex = 0;
                if (options.Id.HasValue)
                {
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} (Id = @Id)");
                }
                if (options.Ids != null)
                {
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} (Id in @Ids)");
                }
                if (options.ExcludeIds != null)
                {
                    sql.AppendLine($"{(conditionIndex++ == 0 ? "where" : "and")} (Id not in @ExcludeIds)");
                }
                if (!string.IsNullOrEmpty(options.NormalizedSearch))
                {
                    sql.AppendLine($@"{(conditionIndex++ == 0 ? "where" : "and")} (lower(Name) like lower(@NormalizedSearch))");
                }
                if (!string.IsNullOrEmpty(options.Name))
                {
                    sql.AppendLine($@"{(conditionIndex++ == 0 ? "where" : "and")} (Name = @Name)");
                }
                _logger.LogInformation($"Sql query successfully created:\n{sql.ToString()}");

                _logger.LogInformation("Try to execute sql get deliverys query");
                var result = await QueryAsync<Delivery>(sql.ToString(), options);
                _logger.LogInformation("Sql get deliverys query successfully executed");
                return result;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        public async Task Update(Delivery model)
        {
            try
            {
                _logger.LogInformation("Trying to execute sql update delivery query");
                await ExecuteAsync(@"
                    update Delivery set
                        DeliveryPrice = @DeliveryPrice,
                        Name = @Name
                    where Id = @Id
                ", model);
                _logger.LogInformation("Sql update delivery query successfully executed");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }
    }
}