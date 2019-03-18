using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisCustomAPI.Services;

namespace RedisCustomAPI.Controllers
{
    [Route("api/readonly")]
    [ApiController]
    public class ReadOnlyClientController : ControllerBase
    {
        public IReadOnlyService _service;
        public ReadOnlyClientController(IReadOnlyService service)
        {
            this._service = service;
        }

        [HttpGet("ping")]
        public async Task<IActionResult> Ping()
        {
            var result = await Task.Run(() => _service.Ping());
            return Ok(result);
        }

        [HttpGet("getdata")]
        public async Task<IActionResult> GetDataAsync([FromQuery]string key)
        {
            var result = await Task.Run(() => _service.GetDataAsync(key));
            return Ok(result);
        }

        [HttpGet("appdata")]
        public async Task<IActionResult> GetCacheDataByServiceName([FromQuery] string app)
        {
            try
            {
                var result = await Task.Run(() => _service.GetCacheDataByServiceName(app));
                return Ok(result);
            }
            catch(ArgumentException)
            {
                return NotFound("App not found");
            }
            
        }
        

        [HttpGet("multiappdata")]
        public async Task<IActionResult> GetCacheDataByMultipleServiceNamesAsync([FromQuery] string apps)
        {
            string[] appNames = apps.Split(",");
            var result = await Task.Run(() =>
                        _service.GetCacheDataByMultipleServiceNames(new List<string>(appNames)));
            return Ok(result);
        }
    }
}
