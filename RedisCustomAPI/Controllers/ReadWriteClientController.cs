using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisCustomAPI.Services;

namespace RedisCustomAPI.Controllers
{
    [Route("api/readwrite")]
    [ApiController]
    public class ReadWriteClientController : ControllerBase
    {
        public IReadWriteService _service;
        public ReadWriteClientController(IReadWriteService service)
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
        public async Task<IActionResult> GetData([FromQuery]string key)
        {
            var result = await Task.Run(() => _service.GetData(key));
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
        public async Task<IActionResult> GetCacheDataByMultipleServiceNames([FromQuery] string apps)
        {
            string[] appNames = apps.Split(",");
            var result = await Task.Run(() =>
                    _service.GetCacheDataByMultipleServiceNames(new List<string>(appNames)));
            return Ok(result);
        }

        [HttpPut("setdata")]
        public async Task<IActionResult> SetData([FromQuery] string key, [FromQuery] string value)
        {
            var result = await Task.Run(() =>
                    _service.SetData(new Models.RedisEntry(key, value)));
            return Ok(result);
        }
        [HttpDelete("deleteall")]
        public async Task<IActionResult> FlushAll()
        {
            await Task.Run(() => _service.FlushAll());
            return Ok();
        }
    }
}
