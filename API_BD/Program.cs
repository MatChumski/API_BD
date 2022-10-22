using API_BD.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*
 * El builder permite agregar servicios
 * Contexto de la base de datos <WEB_APIContext> es la clase en el explorador de soluciones
 * UseSqlServer porque se utiliza SqlServer como manejador, tendría que revisarse la documentación
 */
builder.Services.AddDbContext<WEB_APIContext>(obj => obj.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL")));

/*
 * Ignorar bucles dentro de los objetos JSON que pueden hacer referencia a objetos que ya 
 * pertenecen a ellos mismos
 */
builder.Services.AddControllers().AddJsonOptions(opt =>
{ opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
