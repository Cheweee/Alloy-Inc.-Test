using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TestApp.Shared.Enumerations;
using TestApp.Data.Interfaces;
using TestApp.Data.Models;

namespace TestApp.Data.DataAccessObjects.Postgres
{
    public class UserLocationDao : BaseDao, IUserLocationDao
    {
        public UserLocationDao(string connectionString, ILogger logger, DatabaseProvider provider) : base(connectionString, logger, provider) { }

        public async Task Create(IEnumerable<UserLocation> model)
        {
            try
            {
                _logger.LogInformation("Trying to execute sql create user location query");
                await ExecuteAsync($@"
                        insert into{"\"UserLocation\""} (
                           {"\"IPAddress\""},
                           {"\"Location\""}
                        ) values (
                            @IPAddress,
                            @Location
                        );
                ", model);
                _logger.LogInformation("Sql create user location query successfully executed");
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
                _logger.LogInformation("Trying to execute sql delete user locations query");
                await ExecuteAsync($@"
                    delete from{"\"UserLocation\""}
                    where{"\"Id\""} = any(@ids)
                ", new { ids });
                _logger.LogInformation("Sql delete user locations query successfully executed");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        public async Task<IEnumerable<UserLocation>> Get(UserLocationGetOptions options)
        {
            try
            {
                StringBuilder sql = new StringBuilder();

                _logger.LogInformation("Try to create get user locations sql query");

                sql.AppendLine($@"
                    select 
                        {"\"Id\""},
                        {"\"IPAddress\""},
                        {"\"Location\""}
                    from {"\"UserLocation\""}
                ");

                int conditionIndex = 0;
                if (!string.IsNullOrEmpty(options.NormalizedSearch))
                {
                    sql.AppendLine($@"{(conditionIndex++ == 0 ? "where" : "and")} (lower({"\"IPAddress\""}) like lower(@NormalizedSearch)
                    or lower({"\"Location\""}) like lower(@NormalizedSearch)
                    )");
                }
                _logger.LogInformation($"Sql query successfully created:\n{sql.ToString()}");

                _logger.LogInformation("Try to execute sql get user locations query");
                var result = await QueryAsync<UserLocation>(sql.ToString(), options);
                _logger.LogInformation("Sql get user locations query successfully executed");
                return result;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        public async Task Update(IEnumerable<UserLocation> model)
        {
            try
            {
                _logger.LogInformation("Trying to execute sql update user location query");
                await ExecuteAsync($@"
                    update{"\"UserLocation\""} set
                        {"\"IPAddress\""} = @IPAddress,
                       {"\"Location\""} = @Location
                    where{"\"Id\""} = @Id
                ", model);
                _logger.LogInformation("Sql update user location query successfully executed");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }
    }
}