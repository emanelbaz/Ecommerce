using Ecommece.API.Helpers;
using Ecommece.Core.Interfaces;
using Ecommece.EF.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// بيسجل لو فيه خطاء حصل اثناء الmigration 
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
    

    try
    {
        var context =serviceProvider.GetRequiredService<Context>();
        context.Database.MigrateAsync().GetAwaiter().GetResult();
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger("StartupLogger");
        logger.LogError(ex, "An error occurred during startup scoped operations");
    }
}


app.Run();
