using System;
using Xunit;
using Xunit.Abstractions;
using RedisCustomAPI.Services;
using RedisCustomAPI.Models;
using System.Collections.Generic;

namespace LocalServiceTest
{
    public class LocalServiceShould
    {
        private readonly ITestOutputHelper _output;
        private readonly ILocalService _service;

        public LocalServiceShould(ITestOutputHelper output)
        {
            _output = output;
            _service = new LocalService();
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
            // assert
        }

        [Fact]
        public void SetData()
        {
            // arrange
            var sut = _service;
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
            var data = new RedisEntry("fname", "Preetham");
            _service.FlushAll();
            sut.SetData(data);
            // act
            var result = sut.GetData(data.Key);
            var temp = sut.GetData("no such key");
            // assert
            Assert.Equal(data.Value, result);
            Assert.Null(temp);
        }

        [Fact]
        public void GetAllData()
        {
            // arrange
            var sut = _service;
            var data = new RedisDataTable(new Dictionary<string, string>
            {
                {"fname", "Preetham" },
                {"lname", "Chandra" }
            });
            _service.FlushAll();
            foreach(var d in data)
            {
                _service.SetData(new RedisEntry(d.Key, d.Value));
            }
            _service.SetData(new RedisEntry("garbage", "Don't fetch this"));

            // act
            var result = sut.GetAllData(new List<string>(data.Keys));
            // assert
            Assert.Equal(data, result);
        }

        [Fact]
        public void GetAllAppData()
        {
            // arrange
            var sut = _service;
            var appX = new RedisDataTable(new Dictionary<string, string>
            {
                { "app1", "data 1" },
                { "app2", "data 2" },
                { "app3", "data 3" }
            });
            var appY = new RedisDataTable(new Dictionary<string, string>
            {
                { "not_app1", "not_app 1" },
                { "not_app2", "not_app 2" },
                { "not_app3", "not_app 3" }
            });
            _service.FlushAll();
            foreach (var d in appX)
            {
                _service.SetData(new RedisEntry(d.Key, d.Value));
            }
            foreach (var d in appY)
            {
                _service.SetData(new RedisEntry(d.Key, d.Value));
            }

            // act
            var result1 = sut.GetAllAppData("app");
            var result2 = sut.GetAllAppData("not_app");
            var ex = Assert.Throws<ArgumentException>(() => sut.GetAllAppData("No such app"));
            // assert
            Assert.Equal(appX, result1);
            Assert.Equal(appY, result2);
            Assert.Equal("App not found", ex.Message);
        }

    }
}
