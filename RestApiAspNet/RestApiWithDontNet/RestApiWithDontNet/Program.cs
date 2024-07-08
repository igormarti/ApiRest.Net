using EvolveDb;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using RestApiWithDontNet.Business;
using RestApiWithDontNet.Business.Impl;
using RestApiWithDontNet.Models.Context;
using RestApiWithDontNet.Repository;
using RestApiWithDontNet.Repository.Impl;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

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

// Versioning Api
builder.Services.AddApiVersioning();

// Services Dependency Injection
builder.Services.
     AddScoped<IUserBusiness, UserBusiness>()
    .AddScoped<IBookBusiness, BookBusiness>();

// Repositories Dependency Injection
builder.Services.
    AddScoped<IUserRepository, UserRepository>()
   .AddScoped<IBookRepository, BookRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

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