using Aggregation.Api.Data;
using Aggregation.Api.Repositories;
using Electricity.Api.Data;
using Electricity.Api.Services.CSVProcessor;
using Electricity.Api.Services.Electicities;
using Electricity.Api.Services.HtmlParser;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<ElectricityDbContext>(x =>
                                                    x.UseSqlServer(builder.Configuration.GetConnectionString("ElectricityDatabase")));

builder.Services.AddScoped<IElectricityRepository, ElectricityRepository>();
builder.Services.AddScoped<IHtmlParserService, HtmlParserService>();
builder.Services.AddScoped<ICSVProcessorService, CSVProcessorService>();
builder.Services.AddScoped<IDbContextProvider, DbContextProvider>();
builder.Services.AddScoped<IElectricityService, ElectricityService>();

var app = builder.Build();

Migrate(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void Migrate(IApplicationBuilder app)
{
    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
    {
        serviceScope.ServiceProvider.GetService<ElectricityDbContext>().Database.Migrate();
    }
}
