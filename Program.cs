using FamilyTree.BL.Services;
using FamilyTree.Data;
using FamilyTree.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddServices(builder.Configuration);

// Configure MVC controllers and JSON serialization to use PascalCase
var mvcBuilder = builder.Services.AddControllers(options =>
{
    // Add global filter that will authorize all endpoints
    // Empty roles array means any authenticated user can access
    options.Filters.Add(new AuthorizeAttribute(true, new string[] { "Admin" }));
    // We will handle authorization at the controller/action level or via custom logic
    // options.Filters.Add(new AuthorizeAttribute());
});

mvcBuilder.AddJsonOptions(options =>
{
    // Remove the default camelCase naming policy
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.DictionaryKeyPolicy = null;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Access configuration
var configuration = builder.Configuration;

string connectionString = configuration.GetConnectionString("Default");

builder.Services.AddDbContext<DataContext>(options =>
 options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


// Read the key from configuration
var secretKey = builder.Configuration["JwtSettings:Key"];

builder.Services.AddSingleton<ITokenService>(new TokenService(secretKey));

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Family Tree", Version = "v1" });

    // Add JWT Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
                      "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                      "Example: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Family Tree  ");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();

app.MapControllers();

app.Run();
