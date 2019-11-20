using System.Threading.Tasks;
using TestApp.Data.Models;

namespace TestApp.Data.Interfaces
{
    public interface ICartReportDao
    {
        Task<CartReport> GetReport(ReportGetOptions options);
    }
}