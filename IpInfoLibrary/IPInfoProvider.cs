using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

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

                if (ipDetails.TryGetProperty("success", out var success) && success.GetBoolean() == false)
                {
                    var errorInfo = ipDetails.GetProperty("error");
                    var errordetails = new IPDetails
                    {
                        Error = errorInfo.ToString(),
                    };
                    return errordetails;
                }

                var details = new IPDetails
                {
                    City = ipDetails.GetProperty("city").GetString(),
                    Country = ipDetails.GetProperty("country_name").GetString(),
                    Continent = ipDetails.GetProperty("continent_name").GetString(),
                    Latitude = ipDetails.GetProperty("latitude").GetDouble().ToString(),
                    Longitude = ipDetails.GetProperty("longitude").GetDouble().ToString(),
                    Error = "none"
                };

                return details;
            }
            catch (Exception ex)
            {
                throw new IPServiceNotAvailableException("error occurred", ex);
            }
        }
    }
}
