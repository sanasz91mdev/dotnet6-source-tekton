using DataAccess.EFCore.CCA;
using DTO.Requests;
using DTO.Response;
using Microsoft.Extensions.Logging;
using Services.BusinessLogic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DigitalBanking.Modules
{
    public class CardModule : ICarterModule
    {
        public record GitHubBranch(
    [property: JsonPropertyName("name")] string Name);
        ILogger _logger;
        Card card;

        public CardModule(ILogger<CardModule> logger)
        {
            _logger = logger;
        }

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("v1/Cards", getCards);
            app.MapGet("v2/Cards", getCardsViaAPI);
        }




        public async Task<IResult> getCardsViaAPI(string customerId, Card cardService, IHttpClientFactory httpClientFactory)
        {
            try
            {
                //calL API -- this is to demonstrate how Http calls are traced ,
                //and Http calls metrics are also automatically exported


                var httpClient = httpClientFactory.CreateClient("GitHub");
                var httpResponseMessage = await httpClient.GetAsync(
                    "repos/dotnet/AspNetCore.Docs/branches");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    using var contentStream =
                        await httpResponseMessage.Content.ReadAsStreamAsync();

                    IEnumerable<GitHubBranch> GitHubBranches = await JsonSerializer.DeserializeAsync
                          <IEnumerable<GitHubBranch>>(contentStream);
                }

                card = cardService;
                _logger.LogInformation("Calling Card service to get card");
                //Card card = new Card(context);
                var request = new CardRequest { CustomerID = customerId.ToString() };
                var response = card.GetCard(request);
                if (response.CardNumber == null)
                {
                    return Results.NotFound(new Error { Message = "Card not found", ResponseCode = "023" });
                }
                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                return Results.Json(data: new Error
                {
                    Message = "Failed to process request.",
                    StackTrace = ex.StackTrace.ToString(),
                    ResponseCode = "199",
                    ExceptionMessage = ex.Message
                }, statusCode: 500);
            }
        }


        public async Task<IResult> getCards(string customerId, Card cardService)
        {
            try
            {
                card = cardService;
                _logger.LogInformation("Calling Card service to get card");
                //Card card = new Card(context);
                var request = new CardRequest { CustomerID = customerId.ToString() };
                var response = card.GetCard(request);
                if (response.CardNumber == null)
                {
                    return Results.NotFound(new Error { Message = "Card not found", ResponseCode = "023" });
                }
                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                return Results.Json(data: new Error
                {
                    Message = "Failed to process request.",
                    StackTrace = ex.StackTrace.ToString(),
                    ResponseCode = "199",
                    ExceptionMessage = ex.Message
                }, statusCode: 500);
            }
        }

    }
}

