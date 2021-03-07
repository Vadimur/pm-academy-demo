using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Task_2
{
    public class TestsSet
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private readonly IJsonReader _jsonReader;

        public TestsSet(IJsonReader jsonReader)
        {
            _jsonReader = jsonReader;
        }

        private bool ConfigureHttpClient()
        {
            string configBaseUrl;
            try
            {
                configBaseUrl = _jsonReader.ReadBaseUrl();
            }
            catch (DataAccessException exception)
            {
                Console.WriteLine($"Exception occured: {exception.Message}");
                return false;
            }

            try
            {
                HttpClient.BaseAddress = new Uri(configBaseUrl);
            }
            catch (Exception exception)
            {
                if (exception is ArgumentNullException ||
                    exception is UriFormatException)
                {
                    Console.WriteLine($"Exception occured: {exception.Message}");
                    return false;
                }

                throw;
            }

            return true;
        }
        public async Task RunAll()
        {
            bool isConfigured = ConfigureHttpClient();
            if (!isConfigured)
            {
                return;
            }

            Task[] tasks =
            {
                Test_RegisterEndpoint(),
                Test_RatesEndpoint_ValidCurrencies(),
                Test_RatesEndpoint_UnknownCurrency()
            };

            await Task.WhenAll(tasks);
        }
        public async Task Test_RegisterEndpoint()
        {
            await BaseTest(1, HttpMethod.Post, "/register", 200, 
                "{\"message\":\"Sorry, we cannot register you right now. Please, try again later\",\"code\":1}",
                "{\"login\": \"string\",\"password\": \"string\"}");
        }
        
        public async Task Test_RatesEndpoint_ValidCurrencies()
        {
            await BaseTest(2, HttpMethod.Get, "/Rates/USD/EUR", 200); 
            // not checking response body because it changes
        }
        
        public async Task Test_RatesEndpoint_UnknownCurrency()
        {
            await BaseTest(3, HttpMethod.Get, "/Rates/XYZ/EUR", 400, "Invalid currency code"); 
        }
        
        private async Task BaseTest(
            int testNumber,
            HttpMethod method,
            string endpoint,
            int expectedResponseStatusCode,
            string expectedResponseBody = null,
            string requestBody = null)
        {
            //test info | arrange
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"----------------------Test #{testNumber}----------------------");
            builder.AppendLine($"Testing '{endpoint}' endpoint");
            if (expectedResponseBody != null)
            {
                builder.AppendLine($"Expected response body: '{expectedResponseBody}'");
            }
            else
            {
                builder.AppendLine("Not checking response body, because it is dynamic");
                
            }
            builder.AppendLine($"Expected response status code: '{expectedResponseStatusCode}'");
            builder.AppendLine();

            //sending request | act
            HttpResponseMessage response;
            try
            {
                response = await SendRequest(method, endpoint, requestBody);
            }
            catch (HttpRequestException exception)
            {
                builder.AppendLine($"Exception occured: {exception.Message}");
                ShowTestSummary(builder);

                return;
            }

            string actualResponseBody = await response.Content.ReadAsStringAsync();
            int actualResponseStatusCode = (int)response.StatusCode;

            // checking expected adn actual results | assert
            if (expectedResponseBody == null)
            {
                builder.AppendLine($"Response body: '{actualResponseBody}'");
            }
            else if (expectedResponseBody.Equals(actualResponseBody))
            {
                builder.AppendLine("Response body matches expected value");
            }
            else
            {
                builder.AppendLine("Response body doesn't match expected value");
                builder.AppendLine($"Actual response body: '{actualResponseBody}'");
            }

            if (expectedResponseStatusCode == actualResponseStatusCode)
            {
                builder.AppendLine("Response status code matches expected value");
            }
            else
            {
                builder.AppendLine("Response status code doesn't match expected value");
                builder.AppendLine($"Actual response status code: '{actualResponseStatusCode}'");
            }

            ShowTestSummary(builder);
        }

        private async Task<HttpResponseMessage> SendRequest(HttpMethod method, string endpoint, string body = null)
        {
            if (method == HttpMethod.Get)
            {
                return await HttpClient.GetAsync(endpoint);
            }
            if (method == HttpMethod.Post)
            {
                return await HttpClient.PostAsync(endpoint, new StringContent(body, Encoding.UTF8, "application/json"));
            }
            
            throw new ArgumentException("Unsupported method");
        }
        
        private void ShowTestSummary(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine();
            string testSummary = stringBuilder.ToString();
            Console.WriteLine(testSummary);
        }
    }
}
