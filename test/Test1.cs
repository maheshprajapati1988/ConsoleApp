using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Infrastructure.Services;
using System.Net;
using Xunit;

namespace test
{
    public class UserServiceTest
    {
        [Fact]
        public async Task GetUserByIdAsync_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var notFoundResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
            var mockHandler = new MSTestSettings(notFoundResponse);
            var httpClient = new HttpClient(mockHandler);

            var options = Options.Create(new API { BaseUrl = "https://reqres.in/api", ApiKey = "reqres-free-v1" });
            var cache = new MemoryCache(new MemoryCacheOptions());

            var service = new ResponseService(httpClient, options, cache);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await service.GetUserById(999));
        }
    }
}
