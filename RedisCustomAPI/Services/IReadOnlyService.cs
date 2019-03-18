using RedisCustomAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisCustomAPI.Services
{
    public interface IReadOnlyService
    {
        bool Ping();
        string GetData(string key);
        RedisDataTable GetCacheDataByServiceName(string appName);
        RedisDataTable GetCacheDataByMultipleServiceNames(List<string> apps);
    }
}
