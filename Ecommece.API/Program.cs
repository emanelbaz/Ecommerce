using Ecommece.API.Helpers;
using Ecommece.API.Middleware;
using Ecommece.Core.Caching;
using Ecommece.Core.Interfaces;
using Ecommece.Core.Models;
using Ecommece.EF.Data;
using Ecommece.EF.Messaging;
using Ecommece.EF.Repositories;
using Ecommece.EF.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//redis

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
});
// Add services to the container.
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
    );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🧠 JWT Config
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// 🧠 Register TokenService
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddSingleton<IMessageBroker, RabbitMQMessageBroker>();
//add cashing service
builder.Services.AddScoped<ICacheService, RedisCacheService>();
// Add CORS policy
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAngular",
//        policy =>
//        {
//            policy.WithOrigins("http://localhost:4200") // Angular app origin
//                  .AllowAnyHeader()
//                  .AllowAnyMethod();
//        });
//});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAngular",
//       builder =>
//       {
//           builder.WithOrigins("http://localhost:4200", "http://196.219.146.28")
//                  .AllowAnyHeader()
//                  .AllowAnyMethod();
//       });


//});
builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
var app = builder.Build();

//Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
app.UseStaticFiles(); 
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommece API");
//    c.RoutePrefix = string.Empty; // optional لو عايزة swagger يكون الصفحة الرئيسية
//});
app.UseSwaggerUI();
//}

//دا جزء الerror
app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UsePathBase("/");
//app.UseHttpsRedirection();
app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

// بيسجل لو فيه خطاء حصل اثناء الmigration 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();

    try
    {
        var context = services.GetRequiredService<Context>();
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger("StartupLogger");
        logger.LogError(ex, "An error occurred during migration");
    }
}


app.Run();
