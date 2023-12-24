using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SearchService.Extensions;
using SearchService.Services;
using SearchService.Services.Policy.Handler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient<AuctionServiceHttpClient>().AddPolicyHandler(Resilient.GetPolicy());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) => { cfg.ConfigureEndpoints(context); });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#region Add Extensions

// Config the lifetime of the application
app.Lifetime.ApplicationStarted.Register(async () =>
{
    _ = await app.UseAppBuilderExtension(builder.Configuration);
});

#endregion

app.Run();