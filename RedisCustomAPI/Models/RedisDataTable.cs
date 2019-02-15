using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisCustomAPI.Models
{
    public class RedisDataTable : Dictionary<string, string>
    {
        public RedisDataTable(IDictionary<string, string> dictionary) : base(dictionary)
        {
        }
    }
}
