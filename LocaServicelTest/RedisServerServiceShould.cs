using System;
using Xunit;
using Xunit.Abstractions;
using RedisCustomAPI.Services;
using RedisCustomAPI.Models;
using System.Collections.Generic;

namespace ServiceTest
{
    public class RedisServerServiceShould
    {
        private readonly ITestOutputHelper _output;
        private readonly IRedisServerService _service;

        public RedisServerServiceShould(ITestOutputHelper output)
        {
            _output = output;
            _service = new RedisServerService();
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
        public void GetCacheDataByServiceName()
        {
            // arrange
            var sut = _service;

            // act
            var result = sut.GetCacheDataByServiceName("DSA");
            var ex = Assert.Throws<ArgumentException>(() => sut.GetCacheDataByServiceName("No such app"));
            // assert
            foreach(var d in result)
            {
                Assert.StartsWith("DSA", d.Key);
            }
            Assert.Equal("App not found", ex.Message);
        }

        [Fact]
        public void GetCacheDataByMultipleServiceNames()
        {
            // arrange
            var sut = _service;

            // act
            var apps = new List<string>(new string[] { "DSAProductDC", "DSAWebApp" });
            var result = sut.GetCacheDataByMultipleServiceNames(apps);
            // assert
            foreach (var d in result)
            {
                bool error = true;
                foreach (var app in apps)
                {
                    if (d.Key.StartsWith(app))
                    {
                        error = false;
                        break;
                    }
                }
                if (error)
                {
                    Assert.True(false);
                }
            }
            Assert.True(true);
        }
    }
}
