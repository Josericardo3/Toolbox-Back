using inti_repository;
using inti_model;
using MySql.Data.MySqlClient;

const String default_url = "http://{0}:{1}";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var port = Environment.GetEnvironmentVariable("PORT");
var host = Environment.GetEnvironmentVariable("HOST");
var env = Environment.GetEnvironmentVariable("ENV");

String connectionString = env != "DEV" ? "MySqlConnectionDev" : "MySqlConnection";


Console.WriteLine("Connection string {0}", connectionString);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

String to_use_urls = String.Format(default_url, host, port);

Console.WriteLine(to_use_urls);

builder.WebHost.UseUrls(to_use_urls);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var MySqlConfiguration = new MySQLConfiguration(builder.Configuration.GetConnectionString(connectionString));
builder.Services.AddSingleton(MySqlConfiguration);
builder.Services.AddScoped<IUsuarioPstRepository, UsuarioPstRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


