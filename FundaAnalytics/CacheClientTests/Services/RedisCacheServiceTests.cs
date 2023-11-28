using CacheClient.Clients;
using CacheClient.Services;
using CacheClient.Wrappers;
using NSubstitute;
using StackExchange.Redis;
using System.Text.Json;

namespace CacheClientTests.Services
{
    [TestFixture]
    public class RedisCacheServiceTests
    {
        private IRedisConnectionFactory _redisConnectionFactory;
        private IConnectionMultiplexerWrapper _connectionMultiplexerWrapper;
        private IDatabase _redisDatabase;
        private RedisCacheService _cacheService;

        [SetUp]
        public void Setup()
        {
            _redisConnectionFactory = Substitute.For<IRedisConnectionFactory>();
            _connectionMultiplexerWrapper = Substitute.For<IConnectionMultiplexerWrapper>();
            _redisDatabase = Substitute.For<IDatabase>();
            _redisConnectionFactory.GetConnection().Returns(_connectionMultiplexerWrapper);
            _connectionMultiplexerWrapper.GetDatabase().Returns(_redisDatabase);
            _cacheService = new RedisCacheService(_redisConnectionFactory);
        }

        [Test]
        public async Task SetDataAsync_Successful()
        {
            // Arrange
            var key = "testKey";
            var value = new TestData { Id = 1, Name = "TestName" };

            // Act
            await _cacheService.SetDataAsync(key, value);

            // Assert
            await _redisDatabase.Received().StringSetAsync(key, JsonSerializer.Serialize(value, new JsonSerializerOptions()));
        }

        [Test]
        public async Task GetDataAsync_KeyExists_ReturnsValue()
        {
            // Arrange
            var key = "testKey";
            var expectedValue = new TestData { Id = 1, Name = "TestName" };
            _redisDatabase.StringGetAsync(key).Returns(JsonSerializer.Serialize(expectedValue));

            // Act
            var result = await _cacheService.GetDataAsync<TestData>(key);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedValue.Id, result.Id);
            Assert.AreEqual(expectedValue.Name, result.Name);
        }

        [Test]
        public async Task GetDataAsync_KeyNotExists_ReturnsDefault()
        {
            // Arrange
            var key = "nonExistentKey";
            _redisDatabase.StringGetAsync(key).Returns(RedisValue.Null);

            // Act
            var result = await _cacheService.GetDataAsync<TestData>(key);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task StringDataIncrementAsync_Successful()
        {
            // Arrange
            var key = "testKey";

            // Act
            await _cacheService.StringDataIncrementAsync(key);

            // Assert
            await _redisDatabase.Received().StringIncrementAsync(key);
        }

        [Test]
        public async Task DataKeyExpireAsync_Successful()
        {
            // Arrange
            var key = "testKey";
            var timeSpan = TimeSpan.FromMinutes(30);

            // Act
            await _cacheService.DataKeyExpireAsync(key, timeSpan);

            // Assert
            await _redisDatabase.Received().KeyExpireAsync(key, timeSpan);
        }

        private class TestData
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}
