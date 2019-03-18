using RedisCustomAPI.Services;
using System;
using Xunit;
using Xunit.Abstractions;

namespace ServicesDellTest
{
    public class UnitTest1
    {
        readonly string host = "redis-14336.dcfredis-np.us.dell.com";
        readonly int port = 14336;
        readonly string password = "Km3OJbjCLrPOAJyhf4s8HA==";

        private readonly ITestOutputHelper _output;
        private readonly IReadOnlyService _service;

        public UnitTest1(ITestOutputHelper output)
        {
            _output = output;
            _service = new ReadOnlyService(host, port, password, true);
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
        public void GetAppData()
        {
            // arrange
            var sut = _service;
            // act
            var result = sut.GetCacheDataByServiceName("DSA");
            // assert
            //Assert.True(result);
        }

        [Fact]
        public void Temp()
        {
            // arrange
            var sut = _service;
            // act
            var app = "DSACommerceDC";
            var result = sut.Temp(app);
            _output.WriteLine(result.ToString());
            // assert
            //Assert.True(result);
        }
    }
}
