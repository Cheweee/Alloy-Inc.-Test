using System;
using System.Threading.Tasks;
using TestApp.Data.Interfaces;
using TestApp.Data.Models;

namespace TestApp.Services
{
    public class ReportService
    {
        private readonly ICartReportDao _cartReportdao;

        public ReportService(ICartReportDao cartReportDao)
        {
            _cartReportdao = cartReportDao ?? throw new ArgumentException(nameof(cartReportDao));
        }

        public Task<CartReport> GetCartReport(ReportGetOptions options)
        {
            return _cartReportdao.GetReport(options);
        }
    }
}