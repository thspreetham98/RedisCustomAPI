using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisCustomAPI.Models
{
    public class RedisEntry
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public RedisEntry(string key, string val)
        {
            Key = key;
            Value = val;
        }
    }
}
