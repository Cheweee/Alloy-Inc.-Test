using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestApp.Data.Models;
using TestApp.Services;

namespace TestApp.Api.Controllers
{
    [Route("api/delivery")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly DeliveryService _service;

        public DeliveryController(DeliveryService service)
        {
            _service = service;
        }

        
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]DeliveryGetOptions options)
        {
            return Ok(await _service.Get(options));
        }

        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Delivery model)
        {
            string message = await _service.Validate(model);
            if (!string.IsNullOrEmpty(message))
            {
                return BadRequest(new { message });
            }
            return Ok(await _service.Create(model));
        }

        
        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody]Delivery model)
        {
            string message = await _service.Validate(model);
            if (!string.IsNullOrEmpty(message))
            {
                return BadRequest(new { message });
            }
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