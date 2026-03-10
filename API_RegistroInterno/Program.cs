using Microsoft.EntityFrameworkCore;
using API_RegistroInterno.Context;
using API_RegistroInterno.Data;
using API_RegistroInterno.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaConexion")));

builder.Services.AddDbContext<OyDDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaConexionOyD")));

builder.Services.AddScoped<UsuarioClientesExternosData>();

builder.Services.Configure<IdentityValidationOptions>(
    builder.Configuration.GetSection(IdentityValidationOptions.SectionName));
builder.Services.AddHttpClient<IIdentityValidationService, IdentityValidationService>();

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

app.Run();
