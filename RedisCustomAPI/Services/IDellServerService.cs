using RedisCustomAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisCustomAPI.Services
{
    public interface IDellServerService
    {
        bool Ping();
        RedisDataTable GetCacheDataByServiceName(string appName);
    }
}
