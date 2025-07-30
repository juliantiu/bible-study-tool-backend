using BibleStudyTool.Infrastructure.DAL.EF;
using BibleStudyTool.Infrastructure.DAL.Npgsql;
using BibleStudyTool.Infrastructure.Identity;
using BibleStudyTool.Infrastructure.ServiceLayer;
using BibleStudyTool.Infrastructure.ServiceLayer.Interfaces;
using BibleStudyTool.Public.HostedServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

string cnxString = builder.Configuration.GetConnectionString("TestDbLocal")!;

// Add services to the container.

builder.Services.AddHostedService<InitBibleVersionDetailsService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adding verses related to Identity/EntityFramework setup
builder.Services.AddDbContext<BibleReadingDbContext>(
    options => options.UseNpgsql
        (cnxString,
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

// Entity query services
builder.Services.AddScoped
    (typeof(BibleVerseQueries), _ => new BibleVerseQueries(cnxString));

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
