using Microsoft.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Text.Json;

namespace biller_server_portal.libs
{
    public class RestClient
    {
        ILogger<RestClient> _logger;
        IHttpClientFactory _httpClientFactory;

        public RestClient(ILogger<RestClient> logger, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<object> sendAsync(RequestMessage message)
        {
            try
            {
                _logger.LogInformation("RestClient:sendAsync method called");

                var httpClient = _httpClientFactory.CreateClient();

                var httpRequestMessage = new HttpRequestMessage(
                message.Method,
                message.Path);


                if (message?.Headers?.Count > 0)
                {
                    _logger.LogInformation("Request Headers: " + JsonSerializer.Serialize(message.Headers));
                    foreach (var item in message.Headers)
                    {
                        httpRequestMessage.Headers.Add(item.Key, item.Value);
                    }
                }
                else
                {
                    _logger.LogInformation("Request Headers: [NOT PROVIDED]");
                }

                if (message?.Body != null)
                {
                    var bodyJson = JsonSerializer.Serialize(message.Body);
                    _logger.LogInformation("Request Body: " + bodyJson);
                    var stringContent = new StringContent(bodyJson, Encoding.UTF8, "application/json");
                    httpRequestMessage.Content = stringContent;
                }
                else
                {
                    _logger.LogInformation("Request Body: [NOT PROVIDED]");
                }

                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                 if (httpResponseMessage.IsSuccessStatusCode || (httpResponseMessage.StatusCode >= HttpStatusCode.NotFound && httpResponseMessage.StatusCode <= HttpStatusCode.InternalServerError))
                {
                    var jsonString = await httpResponseMessage.Content.ReadAsStringAsync();
                    _logger.LogInformation("Response Received: " + jsonString);
                    var response = JsonSerializer.Deserialize<object>(jsonString);
                    return new {error = false , data = response};
                }
                 else
                {
                    return new { error = true, type = "Error", description = "Failed to perform operation" };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to make API Call to path [{message.Path}]");
                _logger.LogError(ex.Message);
                return new { error = true, type = "Exception", description = ex.Message };
            }
        }

    }

    public class RequestMessage
    {
        public Dictionary<string, string> Headers { get; set; }
        public string? Path { get; set; }
        public Dictionary<string, string>? Body { get; set; }
        public HttpMethod Method { get; set; }
    }

    public class ResponseMessage
    {
        public HttpStatusCode StatusCode { get; set; }
        public object JsonResponse { get; set; }
    }

}
