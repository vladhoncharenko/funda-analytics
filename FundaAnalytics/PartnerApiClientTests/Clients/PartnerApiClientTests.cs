using AutoFixture;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PartnerApiClient.RateLimiters;
using PartnerApiModels.DTOs;
using PartnerApiModels.Models;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http.Json;

namespace PartnerApiClientTests.Clients
{
    [TestFixture]
    public class PartnerApiClientTests
    {
        private IHttpClientFactory _httpClientFactory;
        private ILogger<PartnerApi.Clients.PartnerApiClient> _logger;
        private IRateLimiter _rateLimiter;
        private HttpClient _httpClient;
        private IFixture _fixture;

        private MockHttpMessageHandler _mockHttpMessageHandler;
        private const string TestBaseUrl = "http://test.com";
        private const string PartnerApiBaseUrl = "/PartnerApiBaseUrl";
        private const string PropertyListingUrlTemplate = "/PropertyListingUrlTemplate";
        private const string PropertyListingIdsUrlTemplate = "/PropertyListingIdsUrlTemplate";
        private const string ApiKey = "ApiKey";

        [SetUp]
        public void Setup()
        {
            _httpClientFactory = Substitute.For<IHttpClientFactory>();
            _logger = Substitute.For<ILogger<PartnerApi.Clients.PartnerApiClient>>();
            _rateLimiter = Substitute.For<IRateLimiter>();
            _mockHttpMessageHandler = new();
            _httpClient = new HttpClient(_mockHttpMessageHandler) { BaseAddress = new Uri(TestBaseUrl) };
            _fixture = new Fixture();

            Environment.SetEnvironmentVariable("PartnerApiBaseUrl", PartnerApiBaseUrl);
            Environment.SetEnvironmentVariable("PropertyListingUrlTemplate", PropertyListingUrlTemplate);
            Environment.SetEnvironmentVariable("PropertyListingIdsUrlTemplate", PropertyListingIdsUrlTemplate);
            Environment.SetEnvironmentVariable("ApiKey", ApiKey);

            _fixture.Customize<PropertyListingInfoDto>(pl => pl.With(p => p.PublicatieDatum, "(1701181118)"));
            _fixture.Customize<PropertyListingDto>(pl => pl.With(p => p.PublicatieDatum, "(1701181118)"));
        }

        #region GetPropertyListingIdsAsync

        [Test]
        public async Task GetPropertyListingAsync_ValidId_ReturnsPropertyListing()
        {
            // Arrange
            var client = CreateApiClient();
            var propertyFundaId = _fixture.Create<string>();
            var expectedPropertyListingDto = _fixture.Create<PropertyListingDto>();

            _mockHttpMessageHandler
                .Expect(HttpMethod.Get, TestBaseUrl + PartnerApiBaseUrl + PropertyListingUrlTemplate)
                .Respond(HttpStatusCode.OK, JsonContent.Create(expectedPropertyListingDto));

            _httpClientFactory.CreateClient("HttpClient").Returns(_httpClient);
            _rateLimiter.ShouldLimitRequestAsync("PartnerApi").Returns(false);

            // Act
            var result = await client.GetPropertyListingAsync(propertyFundaId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<PropertyListing>(result);
        }

        [Test]
        public async Task GetPropertyListingAsync_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var client = CreateApiClient();
            string invalidPropertyFundaId = null;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(() => client.GetPropertyListingAsync(invalidPropertyFundaId));
        }

        #endregion

        #region GetPropertyListingIdsAsync

        [Test]
        public async Task GetPropertyListingIdsAsync_ReturnsPropertyListingIds()
        {
            // Arrange
            var client = CreateApiClient();
            var page = 1;
            var expectedPropertyListingIdsDto = _fixture.Create<PropertyListingIdsDto>();

            _mockHttpMessageHandler
                .Expect(HttpMethod.Get, TestBaseUrl + PartnerApiBaseUrl + PropertyListingIdsUrlTemplate)
                .Respond(HttpStatusCode.OK, JsonContent.Create(expectedPropertyListingIdsDto));

            _httpClientFactory.CreateClient("HttpClient").Returns(_httpClient);
            _rateLimiter.ShouldLimitRequestAsync("PartnerApi").Returns(false);

            // Act
            var result = await client.GetPropertyListingIdsAsync(page);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<PropertyListingIds>(result);
        }

        [Test]
        public async Task GetPropertyListingIdsAsync_InvalidParams_ReturnsPropertyListingIds_WithDefaultParams()
        {
            // Arrange
            var client = CreateApiClient();
            var page = -1;
            var pageSize = -1;
            var expectedPropertyListingIdsDto = _fixture.Create<PropertyListingIdsDto>();

            _mockHttpMessageHandler
                .Expect(HttpMethod.Get, TestBaseUrl + PartnerApiBaseUrl + PropertyListingIdsUrlTemplate)
                .Respond(HttpStatusCode.OK, JsonContent.Create(expectedPropertyListingIdsDto));

            _httpClientFactory.CreateClient("HttpClient").Returns(_httpClient);
            _rateLimiter.ShouldLimitRequestAsync("PartnerApi").Returns(false);

            // Act
            var result = await client.GetPropertyListingIdsAsync(page, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<PropertyListingIds>(result);
        }

        #endregion

        private PartnerApi.Clients.PartnerApiClient CreateApiClient()
        {
            return new PartnerApi.Clients.PartnerApiClient(_httpClientFactory, _logger);
        }
    }
}