using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedisCustomAPI.Services;

namespace RedisCustomAPI.Controllers
{
    [Route("api/local/readwrite")]
    [ApiController]
    public class ReadWriteClientController : ControllerBase
    {
        public IReadWriteService _service;
        public ReadWriteClientController(IReadWriteService service)
        {
            this._service = service;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            var result = _service.Ping();
            return Ok(result);
        }

        [HttpGet("getdata")]
        public IActionResult GetData([FromQuery]string key)
        {
            var result = _service.GetData(key);
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

        [HttpPut("setdata")]
        public IActionResult SetData([FromQuery] string key, [FromQuery] string value)
        {
            var result = _service.SetData(new Models.RedisEntry(key, value));
            return Ok(result);
        }
        [HttpDelete("deleteall")]
        public IActionResult FlushAll()
        {
            _service.FlushAll();
            return Ok();
        }
    }
}
