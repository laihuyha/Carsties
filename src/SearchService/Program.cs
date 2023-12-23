using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SearchService.Extensions;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient<AuctionServiceHttpClient>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#region Add Extensions

await app.UseAppBuilderExtension(builder.Configuration);

#endregion

app.Run();
