using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RedisCustomAPI.Models;
using ServiceStack.Redis;

namespace RedisCustomAPI.Services
{
    public class ReadWriteService : ReadOnlyService, IReadWriteService
    {
        public ReadWriteService(string host, int port, string password)
            : base(host, port, password){}


        public void FlushAll()
        {
            using (IRedisClient client = new RedisClient(_host, _port, _password))
            {
                try
                {
                    client.FlushAll();
                }
                catch (RedisException)
                {
                    return;
                }
            }
        }

        public bool SetData(RedisEntry data)
        {
            using (IRedisClient client = new RedisClient(_host, _port, _password))
            {
                try
                {
                    client.Set(data.Key, data.Value);
                    return true;
                }
                catch (RedisException)
                {
                    return false;
                }
            }
        }
    }
}
