using System;
using Xunit;
using Xunit.Abstractions;
using RedisCustomAPI.Services;
using RedisCustomAPI.Models;
using System.Collections.Generic;

namespace ServicesTestLocal
{
    public class ReadWriteServiceShould
    {
        readonly string host = "127.0.0.1";
        readonly int port = 6379;
        readonly string password = null;

        private readonly ITestOutputHelper _output;
        private readonly IReadWriteService _service;

        public ReadWriteServiceShould(ITestOutputHelper output)
        {
            _output = output;
            _service = new ReadWriteService(host, port, password);
        }

        [Fact]
        public void Ping()
        {
            // arrange
            var sut = _service;
            // act
            var result = sut.Ping();
            // assert
            Assert.True(result);
        }

        [Fact]
        public void FlushAll()
        {
            // arrange
            var sut = _service;
            // act
            sut.FlushAll();
            //sut.FlushAll();
            // assert
        }

        [Fact]
        public void SetData()
        {
            // arrange
            var sut = _service;
            sut.FlushAll();
            var data = new RedisEntry("fname", "Preetham");
            // act
            var result = sut.SetData(data);
            // assert
            Assert.True(result);
        }

        [Fact]
        public void GetData()
        {
            // arrange
            var sut = _service;
            sut.FlushAll();
            var data = new RedisEntry("fname", "Preetham");
            sut.SetData(data);
            // act
            var result = sut.GetData(data.Key);
            // assert
            Assert.Equal(result, data.Value);
        }

        [Fact]
        public void GetCacheDataByServiceName()
        {
            // arrange
            var sut = _service;
            sut.FlushAll();
            var data = new RedisDataTable(new Dictionary<string, string>())
            {
                { "app1", "data1" },
                { "app2", "data2" },
                { "app3", "data3" }
            };
            foreach (var d in data)
            {
                sut.SetData(new RedisEntry(d.Key, d.Value));
            }
            sut.SetData(new RedisEntry("Garbage", "Don't fetch this"));
            // act
            var result = sut.GetCacheDataByServiceName("app");
            // assert
            Assert.Equal(result, data);
        }

        [Fact]
        public void GetCacheDataByMultipleServiceNames()
        {
            // arrange
            var sut = _service;
            sut.FlushAll();
            var data = new RedisDataTable(new Dictionary<string, string>())
            {
                { "appX1", "data1" },
                { "appX2", "data2" },
                { "appX3", "data3" },
                { "appY1", "data4" },
                { "appY2", "data5" },
                { "appY3", "data6" }
            };
            foreach (var d in data)
            {
                sut.SetData(new RedisEntry(d.Key, d.Value));
            }
            sut.SetData(new RedisEntry("Garbage", "Don't fetch this"));
            // act
            var result = sut.GetCacheDataByMultipleServiceNames(new List<string>(new string[] {"appX", "appY" }));
            // assert
            Assert.Equal(result, data);
        }
    }
}
