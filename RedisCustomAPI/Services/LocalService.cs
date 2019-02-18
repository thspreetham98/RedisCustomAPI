using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RedisCustomAPI.Models;
using ServiceStack.Redis;

namespace RedisCustomAPI.Services
{
    public class LocalService : ILocalService
    {
        public string Host { get => "127.0.0.1"; }
        public int Port { get => 6379; }
        public string Password { get => null; }

        public void FlushAll()
        {
            using (IRedisClient client = new RedisClient(Host, Port, Password))
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

        public RedisDataTable GetAllAppData(string appName)
        {
            if (appName == null)
            {
                throw new ArgumentNullException("appName cannot be null");
            }
            if (appName.Length < 1)
            {
                throw new ArgumentException("Enter at least one letter");
            }
            appName += "*";
            using (IRedisClient client = new RedisClient(Host, Port, Password))
            {
                try
                {
                    var keys = client.ScanAllKeys(appName);
                    if(keys.Count() == 0)
                    {
                        throw new ArgumentException("App not found");
                    }
                    return new RedisDataTable(client.GetAll<string>(keys));
                }
                catch (RedisException)
                {
                    return null;
                }
            }
        }

        public RedisDataTable GetAllData(List<string> keys)
        {
            if (keys.Count < 1)
            {
                throw new ArgumentException("Pass at least one key");
            }
            using (IRedisClient client = new RedisClient(Host, Port, Password))
            {
                try
                {
                    return new RedisDataTable(client.GetAll<string>(keys));
                }
                catch (RedisException)
                {
                    return null;
                }
            }
        }

        public string GetData(string key)
        {
            using (IRedisClient client = new RedisClient(Host, Port, Password))
            {
                try
                {
                    return client.Get<string>(key);
                }
                catch (RedisException)
                {
                    return null;
                }
            }
        }

        public bool Ping()
        {
            using (IRedisClient client = new RedisClient(Host, Port, Password))
            {
                try
                {
                    client.Ping();
                    return true;
                }
                catch (RedisException)
                {
                    return false;
                }
            }
        }

        public bool SetData(RedisEntry data)
        {
            using (IRedisClient client = new RedisClient(Host, Port, Password))
            {
                try
                {
                    client.Set<string>(data.Key, data.Value);
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
