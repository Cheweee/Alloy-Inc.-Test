using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestApp.Data.Models;
using TestApp.Services;

namespace TestApp.Api.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _service;

        public OrderController(OrderService service)
        {
            _service = service;
        }

        
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]OrderGetOptions options)
        {
            return Ok(await _service.Get(options));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Order model)
        {
            string message = _service.Validate(model);
            if (!string.IsNullOrEmpty(message))
            {
                return BadRequest(new { message });
            }
            return Ok(await _service.Create(model));
        }
        
        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody]Order model)
        {
            return Ok(await _service.Update(model));
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody]IReadOnlyList<int> ids)
        {
            await _service.Delete(ids);

            return Ok();
        }
    }
}