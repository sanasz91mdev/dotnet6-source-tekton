using Cap.OpenTelemetry;
using DigitalBanking.Services.Contracts;
using DigitalBanking.Services.Implementation;
using Services;
using Services.BusinessLogic;

namespace DigitalBanking.ServiceExtensions
{
    public static partial class ResourceServices
    {
        public static WebApplicationBuilder UseResourceServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddLogging();
            builder.Services.AddTransient<IOTPService, OneTimePinService>();
           builder.Services.AddTransient<Card>();
            builder.Services.AddTransient<Span>();
            builder.Services.AddSingleton<Mask>();
            return builder;
        }
    }
}
