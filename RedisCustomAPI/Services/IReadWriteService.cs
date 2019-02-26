using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RedisCustomAPI.Models;

namespace RedisCustomAPI.Services
{
    public interface IReadWriteService : IReadOnlyService
    {
        void FlushAll();
        bool SetData(RedisEntry data);   
    }
}
