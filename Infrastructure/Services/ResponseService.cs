using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Core.Models;
namespace Infrastructure.Services
{
    public class ResponseService
    {
        private readonly HttpClient _httpClient;
        private readonly API _options;
        private readonly IMemoryCache _cache;
        private readonly AsyncRetryPolicy _retryPolicy;

        public ResponseService(HttpClient httpClient, IOptions<API> options, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _cache = cache;

            _retryPolicy = Policy
                .Handle<HttpRequestException>()
                .Or<TaskCanceledException>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _options.ApiKey);
        }

        public async Task<Users> GetUserById(int userId)
        {
            string cacheKey = $"User_{userId}";

            if (_cache.TryGetValue(cacheKey, out Users cachedUser))
            {
                return cachedUser;
            }

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                var response = await _httpClient.GetAsync($"{_options.BaseUrl}/users/{userId}");

                var jsonString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"StatusCode: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var VResponse = JsonConvert.DeserializeObject<User_Detials>(content);

                if (VResponse?.Data == null)
                {
                    throw new Exception("No user found.");
                }

                _cache.Set(cacheKey, VResponse.Data, TimeSpan.FromMinutes(15));

                return VResponse.Data;
            });
        }

        public async Task<IEnumerable<Users>> GetAllUsers(int pageNo)
        {
            string cacheKey = "AllUsers";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<Users> cachedUsers))
            {
                return cachedUsers;
            }

            var allUsers = new List<Users>();

            while (true)
            {
                var response = await _retryPolicy.ExecuteAsync(async () =>
                    await _httpClient.GetAsync($"{_options.BaseUrl}/users?page={pageNo}")
                );

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"StatusCode: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                //var pageData = System.Text.Json.JsonSerializer.Deserialize<UserResponse>(content);
                var pageData = JsonConvert.DeserializeObject<UserResponse>(content);

                if (pageData?.Data == null || !pageData.Data.Any())
                    break;

                allUsers.AddRange(pageData.Data);

                if (pageNo >= pageData.TotalPages)
                    break;

                pageNo++;
            }

            _cache.Set(cacheKey, allUsers, TimeSpan.FromMinutes(15));

            return allUsers;
        }
    }

    public class API
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
    }
}
