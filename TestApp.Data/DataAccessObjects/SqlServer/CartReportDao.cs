using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TestApp.Data.Interfaces;
using TestApp.Data.Models;

namespace TestApp.Data.DataAccessObjects.SqlServer
{
    public class CartReportDao : BaseDao, ICartReportDao
    {
        public CartReportDao(string connectionString, ILogger logger) : base(connectionString, logger) { }

        public async Task<CartReport> GetReport(ReportGetOptions options)
        {
            try
            {
                StringBuilder sql = new StringBuilder();

                _logger.LogInformation("Try to create get cart report sql query");

                sql.AppendLine(@"
                    select 
                    	coalesce(sum(Price * Count), 0) as Summary
                    from Cart
                    where OrderId is null
                ");

                if (options.DateFrom.HasValue)
                {
                    sql.AppendLine($"and (DateCreated >= @DateFrom)");
                }
                if (options.DateTo.HasValue)
                {
                    sql.AppendLine($"and (DateCreated <= @DateTo)");
                }
                _logger.LogInformation($"Sql query successfully created:\n{sql.ToString()}");

                _logger.LogInformation("Try to execute sql get cart report query");
                var result = await QueryFirstOrDefaultAsync<CartReport>(sql.ToString(), options);
                _logger.LogInformation("Sql get cart report query successfully executed");
                return result;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw exception;
            }
        }
    }
}