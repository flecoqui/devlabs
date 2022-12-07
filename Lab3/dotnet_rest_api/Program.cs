using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using dotnet_web_api.Services;
using Microsoft.AspNetCore.Authorization;
using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Options;
using Azure.Data.Tables;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.IdentityModel.Tokens.Jwt;
using dotnet_web_api.Controllers;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;


var builder = WebApplication.CreateBuilder(args);

using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
    .SetMinimumLevel(LogLevel.Trace)
    .AddConsole());

ILogger logger = loggerFactory.CreateLogger<Program>();

builder.Services.AddDataProtection().UseCryptographicAlgorithms(
    new AuthenticatedEncryptorConfiguration
    {
        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
        ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
    });

builder.Services.AddScoped<IAuthorizationDisabledService, AuthorizationDisabledService>();

builder.Services.AddAuthorization(options =>
    {
        // 1. This is how you redefine the default policy
        // By default, it requires the user to be authenticated
        //
        // See https://github.com/dotnet/aspnetcore/blob/30eec7d2ae99ad86cfd9fca8759bac0214de7b12/src/Security/Authorization/Core/src/AuthorizationOptions.cs#L22-L28
        options.DefaultPolicy = new AuthorizationPolicyBuilder()
            .AddRequirements(new AuthorizationDisabledOrAuthenticatedUserRequirement())
            .Build();

        // 2. Define a specific, named policy that you can reference from your [Authorize] attributes
        options.AddPolicy("AuthorizationDisabledOrAuthenticatedUser", builder => builder
            .AddRequirements(new AuthorizationDisabledOrAuthenticatedUserRequirement()));
    });
builder.Services.AddScoped<IAuthorizationHandler, AuthorizationDisabledOrAuthenticatedUserRequirementHandler>();


// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// Add services HttpClient to call gridwich endpoint.
builder.Services.AddHttpClient();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Test API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
// The following line enables Application Insights telemetry collection.
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseDefaultFiles();
app.UseAuthentication();
app.UseAuthorization();



if (app.Environment.IsDevelopment())
    app.MapControllers().WithMetadata(new AllowAnonymousAttribute());
else
    app.MapControllers();

app.MapDefaultControllerRoute();
app.Run();
