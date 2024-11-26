using System.Text.Json;

namespace IPInfoLibrary
{
    public class IPInfoProvider : IIPInfoProvider
    {
        private readonly string _apiKey;
        private const string BaseUrl = "http://api.ipstack.com/";

        public IPInfoProvider(string apiKey)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey), "API key is required.");
        }

        public async Task<IPDetails> GetDetails(string ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
                throw new ArgumentException("IP address cannot be null or empty.", nameof(ip));

            using var client = new HttpClient();
            var url = $"{BaseUrl}{ip}?access_key={_apiKey}";

            try
            {
                var response = await client.GetStringAsync(url);
                var ipDetails = JsonSerializer.Deserialize<JsonElement>(response);

                if (ipDetails.TryGetProperty("success", out var success))
                {
                    throw new IPServiceNotAvailableException($"API Error");
                }

                return new IPDetailsDto
                {
                    City = ipDetails.GetProperty("city").GetString(),
                    Country = ipDetails.GetProperty("country_name").GetString(),
                    Continent = ipDetails.GetProperty("continent_name").GetString(),
                    Latitude = ipDetails.GetProperty("latitude").GetDouble(),
                    Longitude = ipDetails.GetProperty("longitude").GetDouble()
                };
            }
            catch (HttpRequestException ex)
            {
                throw new IPServiceNotAvailableException("API request failed.", ex);
            }
        }
    }
}
