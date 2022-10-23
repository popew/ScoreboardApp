using Microsoft.EntityFrameworkCore;
using ScoreboardApp.Api;
using ScoreboardApp.Application;
using ScoreboardApp.Infrastructure;
using ScoreboardApp.Infrastructure.Extensions;
using ScoreboardApp.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.UseDateOnlyTimeOnlyStringConverters());

builder.Services.AddApplicationservices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();


    app.ExecuteApplicationDbContextMigrations();
}

app.MapHealthChecks("/health");

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();