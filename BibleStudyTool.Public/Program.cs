using BibleStudyTool.Infrastructure.DAL.EF;
using BibleStudyTool.Infrastructure.Identity;
using BibleStudyTool.Infrastructure.ServiceLayer;
using BibleStudyTool.Infrastructure.ServiceLayer.Interfaces;
using BibleStudyTool.Public.HostedServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHostedService<InitBibleVersionDetailsService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adding verses related to Identity/EntityFramework setup
builder.Services.AddDbContext<BibleReadingDbContext>(
    options => options.UseNpgsql
        (builder.Configuration.GetConnectionString
            ("TestDbLocal"),
        db => db.MigrationsAssembly
            ("BibleStudyTool.Infrastructure")
        )
);

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<BibleReader>()
    .AddEntityFrameworkStores<BibleReadingDbContext>();

// Adding services related to entities
builder.Services.AddScoped
    (typeof(IBibleVerseService), typeof(BibleVerseService));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<BibleReader>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
