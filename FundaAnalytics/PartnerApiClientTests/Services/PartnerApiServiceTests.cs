using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using PartnerApi.Clients;
using PartnerApi.Services;
using PartnerApiClient.Exceptions;
using PartnerApiModels.Models;

namespace PartnerApiClientTests.Services
{
    [TestFixture]
    public class PartnerApiServiceTests
    {
        private IPartnerApiClient _partnerApiClient;
        private ILogger<PartnerApiService> _logger;
        private PartnerApiService _partnerApiService;

        [SetUp]
        public void Setup()
        {
            _partnerApiClient = Substitute.For<IPartnerApiClient>();
            _logger = Substitute.For<ILogger<PartnerApiService>>();
            _partnerApiService = new PartnerApiService(_partnerApiClient, _logger);
        }

        [Test]
        public async Task GetPropertyListingIdsAsync_Successful()
        {
            // Arrange
            var propertyListingIdsPage1 = new PropertyListingIds
            {
                PagingInfo = new PagingInfo { TotalPages = 2 },
                PropertyListingInfo = new List<PropertyListingInfo> { new PropertyListingInfo { Id = "1" }, new PropertyListingInfo { Id = "2" } }
            };

            var propertyListingIdsPage2 = new PropertyListingIds
            {
                PagingInfo = new PagingInfo { TotalPages = 2 },
                PropertyListingInfo = new List<PropertyListingInfo> { new PropertyListingInfo { Id = "3" }, new PropertyListingInfo { Id = "4" } }
            };

            _partnerApiClient.GetPropertyListingIdsAsync(1).Returns(propertyListingIdsPage1);
            _partnerApiClient.GetPropertyListingIdsAsync(2).Returns(propertyListingIdsPage2);

            // Act
            var result = await _partnerApiService.GetPropertyListingIdsAsync();

            // Assert
            CollectionAssert.AreEquivalent(new[] { "1", "2", "3", "4" }, result);
        }

        [Test]
        public async Task GetPropertyListingIdsAsync_NullResponseFromApi_ShouldResultInEmptyCollection()
        {
            // Arrange
            _partnerApiClient.GetPropertyListingIdsAsync(Arg.Any<int>()).Returns((PropertyListingIds)null);

            // Act
            var result = await _partnerApiService.GetPropertyListingIdsAsync();

            // Assert
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public async Task GetPropertyListingIdsAsync_Exception_ReturnsEmptyCollection()
        {
            // Arrange
            _partnerApiClient.GetPropertyListingIdsAsync(Arg.Any<int>()).Throws(new Exception("Test exception"));

            // Act
            var result = await _partnerApiService.GetPropertyListingIdsAsync();

            // Assert
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void GetPropertyListingIdsAsync_PartnerApiAccessException_RethrowsException()
        {
            // Arrange
            _partnerApiClient.GetPropertyListingIdsAsync(Arg.Any<int>()).Throws(new PartnerApiAccessException("Test access exception"));

            // Act & Assert
            Assert.ThrowsAsync<PartnerApiAccessException>(async () => await _partnerApiService.GetPropertyListingIdsAsync());
        }
    }
}
