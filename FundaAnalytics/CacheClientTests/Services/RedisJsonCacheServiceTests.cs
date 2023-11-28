using CacheClient.Clients;
using CacheClient.Services;
using CacheClient.Wrappers;
using NRedisStack;
using NSubstitute;
using StackExchange.Redis;
using System.Text.Json;

namespace CacheClientTests.Services
{
    public class RedisJsonCacheServiceTests
    {
        private IRedisConnectionFactory _redisConnectionFactory;
        private IConnectionMultiplexerWrapper _connectionMultiplexerWrapper;
        private IDatabase _redisDatabase;
        private RedisJsonCacheService _jsonCacheService;
        private IJsonCommandsAsync _jsonCacheCommands;
        private IJsonCommandsAsyncWrapper _jsonCacheCommandsWrapper;

        [SetUp]
        public void Setup()
        {
            _jsonCacheCommands = Substitute.For<IJsonCommandsAsync>();
            _jsonCacheCommandsWrapper = Substitute.For<IJsonCommandsAsyncWrapper>();
            _redisConnectionFactory = Substitute.For<IRedisConnectionFactory>();
            _connectionMultiplexerWrapper = Substitute.For<IConnectionMultiplexerWrapper>();
            _redisDatabase = Substitute.For<IDatabase>();
            _redisConnectionFactory.GetConnection().Returns(_connectionMultiplexerWrapper);
            _connectionMultiplexerWrapper.GetDatabase().Returns(_redisDatabase);
            _jsonCacheCommandsWrapper.GetJsonCommandsAsync(_redisConnectionFactory).Returns(_jsonCacheCommands);
            _jsonCacheService = new RedisJsonCacheService(_redisConnectionFactory, _jsonCacheCommandsWrapper);
        }

        [Test]
        public async Task SetJsonDataAsync_Successful()
        {
            // Arrange
            var key = "testKey";
            var path = "testPath";
            var value = new TestData { Id = 1, Name = "TestName" };

            // Act
            await _jsonCacheService.SetJsonDataAsync(key, path, value);

            // Assert
            await _jsonCacheCommands.Received().SetAsync(key, path, JsonSerializer.Serialize(value));
        }

        [Test]
        public async Task GetJsonDataAsync_Successful()
        {
            // Arrange
            var key = "testKey";
            var path = "testPath";
            var expectedValue = new TestData { Id = 1, Name = "TestName" };
            _jsonCacheCommands.GetAsync<TestData>(key, path).Returns(expectedValue);

            // Act
            var result = await _jsonCacheService.GetJsonDataAsync<TestData>(key, path);

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public async Task SetJsonDataAsync_Failed()
        {
            // Arrange
            var key = "testKey";
            var path = "testPath";
            var value = new TestData { Id = 1, Name = "TestName" };
            _jsonCacheCommands.SetAsync(key, path, JsonSerializer.Serialize(value)).Returns(false);

            // Act
            var result = await _jsonCacheService.SetJsonDataAsync(key, path, value);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetJsonDataAsync_Failed()
        {
            // Arrange
            var key = "testKey";
            var path = "testPath";
            _jsonCacheCommands.GetAsync<TestData>(key, path).Returns((TestData)null);

            // Act
            var result = await _jsonCacheService.GetJsonDataAsync<TestData>(key, path);

            // Assert
            Assert.IsNull(result);
        }

        private class TestData
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}
