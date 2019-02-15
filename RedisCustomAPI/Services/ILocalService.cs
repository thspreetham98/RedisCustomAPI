using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RedisCustomAPI.Models;

namespace RedisCustomAPI.Services
{
    public interface ILocalService
    {
        string Host { get; }
        int Port { get; }
        string Password { get; }

        bool Ping();
        void FlushAll();
        bool SetData(RedisEntry data);
        string GetData(string key);
        RedisDataTable GetAllData(List<string> list);
    }
}
