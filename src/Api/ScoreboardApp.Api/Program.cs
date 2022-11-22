using Microsoft.OpenApi.Models;
using ScoreboardApp.Api;
using ScoreboardApp.Application;
using ScoreboardApp.Infrastructure;
using ScoreboardApp.Infrastructure.CustomIdentityService;
using ScoreboardApp.Infrastructure.CustomIdentityService.Extensions;
using ScoreboardApp.Infrastructure.Extensions;
using static CSharpFunctionalExtensions.Result;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationservices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddCustomIdentityService(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.UseDateOnlyTimeOnlyStringConverters();

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Scheme = "bearer",
        Description = "Please insert JWT token into field"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.EnsureIdentityDbMigrations();
    await app.SeedIdentityDataAsync();

    app.ExecuteApplicationDbContextMigrations();
}

app.MapHealthChecks("/health");

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program
{ } // Workaround to make the integration tests to work. See: https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0