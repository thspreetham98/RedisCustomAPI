using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RedisCustomAPI.Models;
using ServiceStack.Redis;

namespace RedisCustomAPI.Services
{
    public class ReadOnlyService : IReadOnlyService
    {
        protected readonly string _host;
        protected readonly int _port;
        protected readonly string _password;

        public ReadOnlyService(string host, int port, string password)
        {
            _host = host;
            _port = port;
            _password = password;
        }

        public RedisDataTable GetCacheDataByMultipleServiceNames(List<string> apps)
        {
            if(apps == null || apps.Count == 0)
            {
                throw new ArgumentNullException("appName cannot be null");
            }
            RedisDataTable result = new RedisDataTable(new Dictionary<string, string>());
            using (IRedisClient client = new RedisClient(_host, _port, _password))
            {
                string appName;
                foreach (var app in apps)
                {
                    appName = app + "*";
                    try
                    {
                        var keys = client.ScanAllKeys(appName);
                        if (keys.Count() == 0)
                        {
                            continue;
                        }
                        client.GetAll<string>(keys).ToList().ForEach(x => result.Add(x.Key, x.Value));
                    }
                    catch (RedisException)
                    {
                        return null;
                    }
                }
            }
            return result;
        }

        public RedisDataTable GetCacheDataByServiceName(string appName)
        {
            if (string.IsNullOrEmpty(appName))
            {
                throw new ArgumentNullException("appName cannot be null");
            }
           
            appName += "*";
            using (IRedisClient client = new RedisClient(_host, _port, _password))
            {
                try
                {
                    var keys = client.ScanAllKeys(appName);
                    if (keys.Count() == 0)
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

        public string GetData(string key)
        {
            using (IRedisClient client = new RedisClient(_host, _port, _password))
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
            using (IRedisClient client = new RedisClient(_host, _port, _password))
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
    }
}
