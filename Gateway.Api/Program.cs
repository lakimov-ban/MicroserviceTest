using Gateway.Api.Middlewares;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Configuration.AddJsonFile("ocelot.json",optional:false,reloadOnChange:true);
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<AuthMiddleware>();

app.UseRouting();
app.UseEndpoints(x => x.MapControllers());

await app.UseOcelot();

app.Run();