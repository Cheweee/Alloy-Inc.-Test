using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestApp.Data.Models;
using TestApp.Services;

namespace TestApp.Api.Controllers
{
    [Route("api/report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _service;

        public ReportController(ReportService service)
        {
            _service = service ?? throw new ArgumentException(nameof(service));
        }

        [HttpGet("cart")]
        public async Task<IActionResult> GetCartReport([FromQuery]ReportGetOptions options)
        {
            return Ok(await _service.GetCartReport(options));
        }
    }
}