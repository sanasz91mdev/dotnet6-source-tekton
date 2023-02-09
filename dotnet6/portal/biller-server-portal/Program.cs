using biller_server_portal.libs;
using biller_server_portal.ServiceExtensions;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddSerilog();

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders | HttpLoggingFields.ResponseHeaders | HttpLoggingFields.ResponseStatusCode;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;

});

builder.Services.AddHttpClient();
builder.Services.AddTransient<RestClient>();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var sessionMode = builder.Configuration.GetValue<string>("session:mode");
var sessionTimeout = builder.Configuration.GetValue<double>("session:timeout");

if (sessionMode.Equals("redis"))
{
    var cacheConfig = builder.Configuration.GetValue<string>("cache:configuration:host");
    var cacheConfigPort = builder.Configuration.GetValue<string>("cache:configuration:port");


    builder.Services.AddDistributedRedisCache(options =>
    {
        options.Configuration = cacheConfig + ":" + cacheConfigPort;
        options.InstanceName = "Session_";
    }
    );
}
else
{
    builder.Services.AddMemoryCache();
}

builder.Services.AddSession(options =>
{
    // 'n' minutes later from last access session will be removed.
    options.IdleTimeout = TimeSpan.FromMinutes(sessionTimeout);
});


var app = builder.Build();
app.UseMiddleware<RequestLogContextMiddleware>();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseHttpLogging();

app.UseRouting();

app.UseAuthorization();


app.UseSession();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
