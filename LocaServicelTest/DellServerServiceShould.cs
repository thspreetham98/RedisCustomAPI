﻿using System;
using Xunit;
using Xunit.Abstractions;
using RedisCustomAPI.Services;
using RedisCustomAPI.Models;
using System.Collections.Generic;

namespace ServiceTest
{
    public class DellServerServiceShould
    {
        private readonly ITestOutputHelper _output;
        private readonly IDellServerService _service;

        public DellServerServiceShould(ITestOutputHelper output)
        {
            _output = output;
            _service = new DellServerService();
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
        public void GetAllAppData()
        {
            // arrange
            var sut = _service;

            // act
            var result = sut.GetAllAppData("DSA");
            var ex = Assert.Throws<ArgumentException>(() => sut.GetAllAppData("No such app"));
            // assert
            foreach(var d in result)
            {
                Assert.StartsWith("DSA", d.Key);
            }
            Assert.Equal("App not found", ex.Message);
        }
    }
}
