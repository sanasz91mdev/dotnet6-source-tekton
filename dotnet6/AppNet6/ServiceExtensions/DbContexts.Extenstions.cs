using DataAccess.EFCore.CCA;
using DataAccess.EFCore.Mask;
using Microsoft.EntityFrameworkCore;

namespace DigitalBanking.ServiceExtensions
{
    public static partial class DbContexts
    {
        public static WebApplicationBuilder AddDbContexts(this WebApplicationBuilder builder)
        {
            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

            builder.Services.AddDbContext<CCAContext>(options =>
            options.UseOracle(config.GetConnectionString("DatabaseConnection"), options => options
            .UseOracleSQLCompatibility("11")));

            builder.Services.AddDbContext<MaskContext>(options =>
            options.UseOracle(config.GetConnectionString("DatabaseConnection"), options => options
            .UseOracleSQLCompatibility("11")));
            return builder;
        }
    }
}
