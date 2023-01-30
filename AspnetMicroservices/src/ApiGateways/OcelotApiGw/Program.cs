using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddLogging(new Action<ILoggingBuilder> { }=> {
//    loggingbuilder.AddConfiguration(builder.Configuration.GetSection("EventBusSettings:HostAddress").Value);
//    //loggingbuilder.AddConfiguration(hostingContext.con);
//});
builder.Services.AddOcelot().AddCacheManager(settings=>settings.WithDictionaryHandle());
//builder.Configuration.AddJsonFile("ocelot.Development.json");
builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json");

builder.Logging.AddConsole();
builder.Logging.AddDebug();
var app = builder.Build();

await app.UseOcelot();
app.Run(); 
