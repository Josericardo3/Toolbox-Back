using inti_repository;
using inti_model;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Any;
using inti_repository.usuario;
using inti_repository.actividad;
using inti_repository.caracterizacion;
using inti_repository.diagnostico;
using inti_repository.listachequeo;
using inti_repository.validaciones;
using inti_repository.planmejora;
using inti_repository.matrizlegal;
using inti_repository.auditoria;
using inti_repository.general;

const String default_url = "http://{0}:{1};https://{2}:{3}"; 

var builder = WebApplication.CreateBuilder(args); 

// Add services to the container.
builder.Services.AddControllers();

//var port = int.Parse(Environment.GetEnvironmentVariable("INTI_BACK_PORT"));

//var host = Environment.GetEnvironmentVariable("INTI_BACK_HOST");
var env = Environment.GetEnvironmentVariable("INTI_BACK_ENV");
var port = 8050;
var host = "0.0.0.0";
String connectionString = env != "DEV" ? "MySqlConnectionDev" : "MySqlConnection";

Console.WriteLine("env->" + connectionString);
Console.WriteLine("Connection string {0}", connectionString);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

String to_use_urls = String.Format(default_url, host, port, host, port + 1);

Console.WriteLine(to_use_urls);

builder.WebHost.UseUrls(to_use_urls);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Inti Back Solutions", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
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

builder.Services.AddCors(


);

var MySqlConfiguration = new MySQLConfiguration(builder.Configuration.GetConnectionString("MySqlConnectionDev"));
builder.Services.AddSingleton(MySqlConfiguration);
builder.Services.AddScoped<IUsuarioPstRepository, UsuarioPstRepository>();
builder.Services.AddScoped<IActividadRepository, ActividadRepository>();
builder.Services.AddScoped<IAsesorRepository, AsesorRepository>();
builder.Services.AddScoped<ICaracterizacionRepository, CaracterizacionRepository>();
builder.Services.AddScoped<IDiagnosticoRepository, DiagnosticoRepository>();
builder.Services.AddScoped<IListaChequeoRepository, ListaChequeoRepository>();
builder.Services.AddScoped<IValidacionesRepository, ValidacionesRepository>();
builder.Services.AddScoped<IPlanMejoraRepository, PlanMejoraRepository>();
builder.Services.AddScoped<IMatrizLegalRepository, MatrizLegalRepository>();
builder.Services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
builder.Services.AddScoped<IGeneralRepository, GeneralRepository>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();




var app = builder.Build();



// Configure the HTTP request pipeline.


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());



app.Run();


