using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestApp.Data.Models;
using TestApp.Services;

namespace TestApp.Api.Controllers
{
    [Route("api/userlocation")]
    [ApiController]
    public class UserLocationController : ControllerBase
    {
        private readonly UserLocationService _service;
        public UserLocationController(UserLocationService userLocationService)
        {
            _service = userLocationService ?? throw new ArgumentException(nameof(userLocationService));
        }
        
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]UserLocationGetOptions options)
        {
            return Ok(await _service.Get(options));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]List<UserLocation> model)
        {
            return Ok(await _service.Create(model));
        }
        
        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody]List<UserLocation> model)
        {
            return Ok(await _service.Update(model));
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery][FromBody]IReadOnlyList<int> ids)
        {
            await _service.Delete(ids);

            return Ok();
        }
    }
}