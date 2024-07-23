using Asp.Versioning;
using EvolveDb;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using RestApiWithDontNet.Business;
using RestApiWithDontNet.Business.Impl;
using RestApiWithDontNet.Configurations;
using RestApiWithDontNet.Data.Converter.Impl;
using RestApiWithDontNet.Hypermedia.Enricher;
using RestApiWithDontNet.Hypermedia.Filters;
using RestApiWithDontNet.Models.Context;
using RestApiWithDontNet.Repository;
using RestApiWithDontNet.Repository.Impl;
using RestApiWithDontNet.Services.Contracts;
using RestApiWithDontNet.Services.Impl;
using Serilog;
using System.Text;

const string AppName = "Rest Api Application";
const string AppVersion = "v1";
const string AppDescription = "Rest Api Application created with .NET 8";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add swagger support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(AppVersion,
        new OpenApiInfo{
            Title = AppName,
            Version = AppVersion,
            Description = AppDescription,
            Contact =  new OpenApiContact{
                Name = "Igor Martins",
                Url = new Uri("https://github.com/igormarti")
            }
        }
     );
});

// Token Configuration
var tokenConfiguration = new TokenConfiguration();
new ConfigureFromConfigurationOptions<TokenConfiguration>(
        builder.Configuration.GetSection("TokenConfiguration")
 ).Configure(tokenConfiguration);
builder.Services.AddSingleton(tokenConfiguration);

// Authentication Configuration
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = tokenConfiguration.Issuer,
        ValidAudience = tokenConfiguration.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguration.Secret))
    };
});
// Authorization Configuration
builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder() 
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser().Build()
        );
});

// CORS Configuration
builder.Services.AddCors(options => 
    options.AddDefaultPolicy(b => 
        b.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    )
);

// DataBase Connection
var strConn = builder.Configuration["MySQLConnection:MySQLConnectionString"];
var version = new MySqlServerVersion(new Version(8, 4, 1));
builder.Services.AddDbContext<MySqlContext>(options =>
    options.UseMySql(strConn, version)
);

// Migrations and datasets
if(builder.Environment.IsDevelopment() && strConn!=null)
{
    migrateDataBase(strConn);
}

// Content Negociation Config
builder.Services.AddMvc(option =>
{
    option.RespectBrowserAcceptHeader = true;
    option.FormatterMappings.SetMediaTypeMappingForFormat("xml", "application/xml");
    option.FormatterMappings.SetMediaTypeMappingForFormat("json", "application/json");
}).AddXmlSerializerFormatters();


// HATEOS Filter
var filterOptions = new HyperMediaFilterOptions();
filterOptions.ContentResponseEnricherList.Add(new UserEnricher());
filterOptions.ContentResponseEnricherList.Add(new BookEnricher());

// Enrich Dependency Injection
builder.Services.AddSingleton(filterOptions);
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Versioning Api
builder.Services.AddApiVersioning();

// Business Dependency Injection
builder.Services.
     AddScoped<IUserBusiness, UserBusiness>()
    .AddScoped<IBookBusiness, BookBusiness>()
    .AddScoped<ILoginBusiness, LoginBusiness>()
    .AddScoped<IFileBusiness, FileBusiness>();

// Service Dependency Injection
builder.Services.AddTransient<ITokenService, TokenService>();

// Repositories Dependency Injection
builder.Services.
    AddScoped<IUserRepository, UserRepository>()
   .AddScoped<IBookRepository, BookRepository>()
   .AddScoped<IUserAuthRepository, UserAuthRepository>();

// VO Dependency Injection
builder.Services.
    AddScoped<UserParser>()
    .AddScoped<BookParser>()
    .AddScoped<UserAuthParser>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint($"/swagger/{AppVersion}/swagger.json", $"{AppName}-{AppVersion}");
});
var option = new RewriteOptions();
option.AddRedirect("^$", "swagger");
app.UseRewriter(option);

app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute("DefaultApi", "v{version=apiVersion}/{controller=values}/{id?}");

app.Run();

void migrateDataBase(string connection)
{
    try
    {
        var evolveConnection = new MySqlConnection(connection);
        var evolve = new Evolve(evolveConnection, Log.Information)
        {
            Locations = new List<string>() { "db/migrations", "db/dataset" },
            IsEraseDisabled = true,
        };
        evolve.Migrate();
    }
    catch (Exception ex)
    {
        Log.Error("DataBase migration failed", ex);
        throw;
    }
}