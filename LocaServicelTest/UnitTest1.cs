using System;
using Xunit;
using Xunit.Abstractions;
using RedisCustomAPI.Services;
using RedisCustomAPI.Models;

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
            var data = new RedisEntry("fname", "Preetham");
            _service.FlushAll();
            sut.SetData(data);
            // act
            //var result = sut.GetData(data.Key);
            // assert
            //Assert.Equal(data.Value, result);
        }

    }
}
