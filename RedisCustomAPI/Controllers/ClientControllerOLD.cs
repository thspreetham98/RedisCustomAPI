using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisCustomAPI.Services;

namespace RedisCustomAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientControllerOLD : ControllerBase
    {
        public IReadOnlyService _service;
        public ClientControllerOLD(IReadOnlyService service)
        {
            this._service = service;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            var result = _service.Ping();
            return Ok(result);
        }

        [HttpGet("appdata")]
        public IActionResult GetCacheDataByServiceName([FromQuery] string app)
        {
            try
            {
                var result = _service.GetCacheDataByServiceName(app);
                return Ok(result);
            }
            catch(ArgumentException)
            {
                return NotFound("App not found");
            }
            
        }
        [HttpGet("multiappdata")]
        public IActionResult GetCacheDataByMultipleServiceNames([FromQuery] string apps)
        {
            string[] appNames = apps.Split(",");
            var result = _service.GetCacheDataByMultipleServiceNames(new List<string>(appNames));
            return Ok(result);
        }
    }
}
