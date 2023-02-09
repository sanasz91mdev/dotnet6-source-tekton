using biller_server_portal.libs;
using biller_server_portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;
using System.Text.Json;

namespace biller_server_portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RestClient _restClient;


        public HomeController(ILogger<HomeController> logger, RestClient restClient)
        {
            _logger = logger;
            _restClient = restClient;
        }

        public IActionResult Index()
        {
            string newSessionValue = "testValue";
            string sessionKey = "testKey";
            HttpContext.Session.SetString(sessionKey, newSessionValue);
            return View();
        }

        public IActionResult Privacy()
        {
            var key = HttpContext.Session.GetString("testKey");
            return View();

        }

        [HttpPost]
        public async Task<JsonResult> PrivacyPost(string data)
        {
            try
            {
                _logger.LogInformation("Post action called [PrivacyPost]");

                Dictionary<string, string> body = new Dictionary<string, string>()
                {
                    {    "documentType", "CNIC"},
                    { "documentNumber", "1234567890125"},
                    { "documentValidityDate", "2030-07-25"},
                    { "name", data }
                };

                RequestMessage message = new RequestMessage()
                {
                    Body = body,
                    Path = "http://localhost:3000/users/list", //base url from appsettings.json + append path
                    Method = HttpMethod.Post
                };

                var response = await _restClient.sendAsync(message);

                return Json(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to perform operation. Error: " + ex.ToString());
                return Json(new { error = true, type= "Exception", description = "Failed to process request" });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}