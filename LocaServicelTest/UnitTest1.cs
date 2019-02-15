using System;
using Xunit;
using Xunit.Abstractions;
using RedisCustomAPI.Services;
using RedisCustomAPI.Models;
using System.Collections.Generic;

namespace LocaServicelTest
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
            // assert
            Assert.Equal(data.Value, result);
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

    }
}
