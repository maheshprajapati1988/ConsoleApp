namespace test
{
    public class MSTestSettings : HttpMessageHandler
    {
        private readonly HttpResponseMessage _mockResponse;

        public MSTestSettings(HttpResponseMessage mockResponse)
        {
            _mockResponse = mockResponse;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_mockResponse);
        }
    }
}
