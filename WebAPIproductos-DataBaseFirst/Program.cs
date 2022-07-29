using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebAPIproductos_DataBaseFirst.Entidades;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DBPROYECTOContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL")));

builder.Services.AddControllers().AddJsonOptions(opt =>
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var misReglasCors = "ReglasCors";
builder.Services.AddCors(opt =>
        opt.AddPolicy(name: misReglasCors, builder =>
            builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(misReglasCors);

app.UseAuthorization();

app.MapControllers();

app.Run();
