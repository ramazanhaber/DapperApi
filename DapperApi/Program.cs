using DapperApi.Helper;
using Microsoft.Data.SqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
IDbConnection defaultConnection = new SqlConnection(defaultConnectionString);

string secondConnectionString = builder.Configuration.GetConnectionString("SecondConnection");
IDbConnection secondConnection = new SqlConnection(secondConnectionString);

builder.Services.AddSingleton(new DatabaseConnections(defaultConnection, secondConnection));


var app = builder.Build();
app.UseCors(builder => builder
.AllowAnyHeader()
.AllowAnyMethod()
.AllowAnyOrigin()
);

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "myapi v1");
    });
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

