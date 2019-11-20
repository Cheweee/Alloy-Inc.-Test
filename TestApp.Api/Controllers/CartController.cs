using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestApp.Data.Models;
using TestApp.Services;

namespace TestApp.Api
{    
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService ?? throw new ArgumentException(nameof(cartService));
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]CartGetOptions options)
        {
            return Ok(await _cartService.Get(options));
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Cart model)
        {
            string message = _cartService.Validate(model);
            if (!string.IsNullOrEmpty(message))
            {
                return BadRequest(new { message });
            }
            return Ok(await _cartService.Update(model));
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody]Cart model)
        {
            string message = _cartService.Validate(model);
            if (!string.IsNullOrEmpty(message))
            {
                return BadRequest(new { message });
            }
            return Ok(await _cartService.AddToCart(model));
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]IReadOnlyList<int> ids)
        {
            await _cartService.Delete(ids);

            return Ok();
        }
    }
}