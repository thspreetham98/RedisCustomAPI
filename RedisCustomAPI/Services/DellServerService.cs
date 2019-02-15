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
    public class DellServerService : IDellServerService
    {
        private string _host;
        private int _port;
        private string _password;

        public DellServerService()
        {
            var strKey = "36fc9e60-c465-11cf-8056-444553540000";
            var strEncrypted = "Km3OJbjCLrPOAJyhf4s8HA==";
            TripleDESCryptoServiceProvider cryptoServiceProvider = new TripleDESCryptoServiceProvider();
            byte[] hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(strKey));
            cryptoServiceProvider.Key = hash;
            cryptoServiceProvider.Mode = CipherMode.ECB;
            byte[] inputBuffer = Convert.FromBase64String(strEncrypted);

            this._host = "redis-14336.dcfredis-np.us.dell.com";
            this._port = 14336;
            this._password = Encoding.ASCII.GetString(cryptoServiceProvider.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length));
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
            using (IRedisClient client = new RedisClient(_host, _port, _password))
            {
                try
                {
                    var keys = client.ScanAllKeys(appName);
                    return new RedisDataTable(client.GetAll<string>(keys));
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
