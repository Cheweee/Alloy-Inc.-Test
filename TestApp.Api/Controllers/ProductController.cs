using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestApp.Data.Models;
using TestApp.Services;

namespace TestApp.Api.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _service;

        public ProductController(ProductService service)
        {
            _service = service;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]ProductGetOptions options)
        {
            return Ok(await _service.Get(options));
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Product model)
        {
            string message = await _service.Validate(model);
            if (!string.IsNullOrEmpty(message))
            {
                return BadRequest(new { message });
            }
            return Ok(await _service.Create(model));
        }
        
        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody]Product model)
        {
            return Ok(await _service.Update(model));
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]IReadOnlyList<int> ids)
        {
            await _service.Delete(ids);

            return Ok();
        }
    }
}