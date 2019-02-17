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
    public class ClientController : ControllerBase
    {
        public IDellServerService _service;
        public ClientController(IDellServerService service)
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
        public IActionResult GetAppData([FromQuery] string app)
        {
            var result = _service.GetAllAppData(app);
            return Ok(result);
        }
    }
}
