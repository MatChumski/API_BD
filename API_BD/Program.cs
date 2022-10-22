using API_BD.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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
 * UseSqlServer porque se utiliza SqlServer como manejador, tendr�a que revisarse la documentaci�n
 */
builder.Services.AddDbContext<WEB_APIContext>(obj => obj.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL")));

/*
 * Ignorar bucles dentro de los objetos JSON que pueden hacer referencia a objetos que ya 
 * pertenecen a ellos mismos
 */
builder.Services.AddControllers().AddJsonOptions(opt =>
{ opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });

/*
 * Esquema de autenticaci�n con Jwt
 */
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(Optional =>
    {
        Optional.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Autenticaci�n y Autorizaci�n EN ESE ORDEN ESPEC�FICO
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapControllers();

app.Run();
