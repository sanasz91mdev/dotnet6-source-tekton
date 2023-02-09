using Cap.OpenTelemetry;
using DataAccess.EFCore.CCA;
using DigitalBanking.Modules;
using DTO.Requests;
using DTO.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.BusinessLogic;
using System;
using System.IO;
using System.Text.Json;
using Xunit;
using Assert = Xunit.Assert;

namespace Digital_Banking_tests
{
    public class DatabaseFixture : IDisposable
    {
        public CCAContext context { get; private set; }
        public MockedLogger<Card> mockedLoggerService = new MockedLogger<Card>();
        public MockedLogger<CardModule> mockedLoggerModule = new MockedLogger<CardModule>();
        public MockedLogger<Span> mockedLoggerSpan = new MockedLogger<Span>();
        public Span mockedSpan = null;
        public Card mockCardService;

        public DatabaseFixture()
        {
            //seed
            var options = new DbContextOptionsBuilder<CCAContext>()
            .UseInMemoryDatabase(databaseName: "IrisDb" + DateTime.Now.ToString())
            .Options;



            // Insert seed data into the database using one instance of the context
            context = new CCAContext(options);

            context.Tbldebitcards.Add(new CardEntity { Cardnumber = "1232546", Customerid = "1", Cardname = "Sana Zehra", RelationshipId = "123422890011" });
            context.Tbldebitcards.Add(new CardEntity { Cardnumber = "4451322", Customerid = "2", Cardname = "Asna Ishrat", RelationshipId = "1233367890222" });
            context.Tbldebitcards.Add(new CardEntity { Cardnumber = "554422", Customerid = "3", Cardname = "Anam Anam", RelationshipId = "1234447890133" });

            context.Tblcustomers.Add(new CustomerEntity { Customerid = "1", Firstname = "sana", Lastname = "zehra" });
            context.Tblcustomers.Add(new CustomerEntity { Customerid = "2", Firstname = "asna", Lastname = "Ishrat" });
            context.Tblcustomers.Add(new CustomerEntity { Customerid = "3", Firstname = "Anam", Lastname = "anam" });

            context.SaveChanges();

            mockedSpan = new Span(mockedLoggerSpan.MockLog.Object);
            mockCardService = new Card(mockedLoggerService.MockLog.Object, context, mockedSpan);

    }



    public void Dispose()
        {
            context.Database.EnsureDeleted(); // Remove from memory
            context.Dispose();
        }
    }


    public class CardModuleTest : IClassFixture<DatabaseFixture>
    {
        Card _unitTesting;
        DatabaseFixture _fixture;

        public CardModuleTest(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
       [Trait("Card Service test","Card Found Case")]
        public void FetchCard_CardFoundTest()
        {
            // _fixture._mockedLoggerService.MockLog.Object,
            _unitTesting = new Card(_fixture.mockedLoggerService.MockLog.Object, _fixture.context, _fixture.mockedSpan);
            CardRequest req = new CardRequest();

            //arrange - data such that it exists in mock Db
            req.CustomerID = "1";

            var x = _unitTesting.GetCard(req);
            Assert.NotNull(x.NameOnCard);

        }

        [Fact]
        [Trait("Card Service test", "Card not found Case")]
        public void FetchCard_CardNotFoundTest()
        {
            CardRequest req = new CardRequest();

            //arrange - data such that it does not exists in mock Db
            req.CustomerID = ("111");

            _unitTesting = new Card(_fixture.mockedLoggerService.MockLog.Object, _fixture.context, _fixture.mockedSpan);
            var x = _unitTesting.GetCard(req);
            Assert.Null(x.NameOnCard);

        }


        [Fact]
        [Trait("Card module test", "Card found Case")]
        public async void Module_CardFound()
        {
            CardModule controller = new CardModule(_fixture.mockedLoggerModule.MockLog.Object);
            var response = await controller.getCards("1", _fixture.mockCardService);
            //var okResult = Assert.IsType<Microsoft.AspNetCore.Http.Result.OkObjectResult>(response); --> was possible in MVC, not in Minimal
            var mockHttpContext = CreateMockHttpContext();
            await response.ExecuteAsync(mockHttpContext);

            // Reset MemoryStream to start so we can read the response.
            mockHttpContext.Response.Body.Position = 0;
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var responseCard = await JsonSerializer.DeserializeAsync<CardResponse>(mockHttpContext.Response.Body, jsonOptions);


            Assert.Equal(200, mockHttpContext.Response.StatusCode);
            Assert.NotNull(responseCard);
            Assert.NotNull(responseCard?.NameOnCard);
            Assert.NotNull(responseCard?.CardNumber);
            Assert.NotNull(responseCard?.MaskedCard);
        }


        [Fact]
      [Trait("Card module test", "Card not found Case")]
        public async void Module_CardNotFound()
        {
            CardModule controller = new CardModule(_fixture.mockedLoggerModule.MockLog.Object);
            var response = await controller.getCards("111", _fixture.mockCardService);
            //var okResult = Assert.IsType<Microsoft.AspNetCore.Http.Result.OkObjectResult>(response);
            //--> this was doable in MVC ..
            //in minimal API following change is required as Results methods are not accessible

            var mockHttpContext = CreateMockHttpContext();
            await response.ExecuteAsync(mockHttpContext);

            // Reset MemoryStream to start so we can read the response.
            mockHttpContext.Response.Body.Position = 0;
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var responseCard = await JsonSerializer.DeserializeAsync<Error>(mockHttpContext.Response.Body, jsonOptions);


            Assert.Equal(404, mockHttpContext.Response.StatusCode);
            Assert.NotNull(responseCard);
            Assert.NotNull(responseCard?.Message);
            Assert.NotNull(responseCard?.ResponseCode);
        }


        [Fact]
        [Trait("Card module test", "Test exception")]
        public async void Module_ExceptionOccurs_ReturnsInternalServerError()
        {
            CardModule controller = new CardModule(_fixture.mockedLoggerModule.MockLog.Object);
            var response = await controller.getCards(null, _fixture.mockCardService);
            //var okResult = Assert.IsType<Microsoft.AspNetCore.Http.Result.OkObjectResult>(response);
            var mockHttpContext = CreateMockHttpContext();
            await response.ExecuteAsync(mockHttpContext);

            // Reset MemoryStream to start so we can read the response.
            mockHttpContext.Response.Body.Position = 0;
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var responseCard = await JsonSerializer.DeserializeAsync<Error>(mockHttpContext.Response.Body, jsonOptions);


            Assert.Equal(500, mockHttpContext.Response.StatusCode);
            Assert.NotNull(responseCard);
            Assert.NotNull(responseCard?.Message);
            Assert.NotNull(responseCard?.ResponseCode);
            Assert.Equal("199",responseCard?.ResponseCode);

        }


        private static HttpContext CreateMockHttpContext() =>
    new DefaultHttpContext
    {
        // RequestServices needs to be set so the IResult implementation can log.
        RequestServices = new ServiceCollection().AddLogging().BuildServiceProvider(),
        Response =
        {
            // The default response body is Stream.Null which throws away anything that is written to it.
            Body = new MemoryStream(),
        },
    };

    }
}
