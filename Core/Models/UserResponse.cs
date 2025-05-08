using Newtonsoft.Json;

namespace Core.Models
{
    public class UserResponse
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("per_page")]
        public int PerPage { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("data")]
        public List<Users> Data { get; set; }

        
        public UserResponse()
        {
            this.Data = new List<Users>();
           
        }
    }
}
