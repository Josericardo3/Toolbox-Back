using inti_repository;
using inti_model;
using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.WebHost.UseUrls("http://0.0.0.0:8050");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var MySqlConfiguration = new MySQLConfiguration(builder.Configuration.GetConnectionString("MySqlConnection"));
builder.Services.AddSingleton(MySqlConfiguration);
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


