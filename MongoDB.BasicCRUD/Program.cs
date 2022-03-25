using Microsoft.AspNetCore.Builder;
using MongoDB.BasicCRUD.Model;
using MongoDB.BasicCRUD.Service;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<BookStoreDatabaseSettings>(
    builder.Configuration.GetSection("BookStoreDatabase"));

// CONFIGURA INJECAO DA INTERFACE DO MONGODB
builder.Services.AddSingleton<BookService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup => setup.SwaggerDoc("v1", new OpenApiInfo()
{
    Description = "CRUD para MongoDB utilizando C# Minimal API",
    Title = "MongoDB CRUD",
    Version = "v1",
    Contact = new OpenApiContact()
    {
        Name = "Luiz Oliveira",
        Url = new Uri("https://github.com/lroliveira1974")
    }
}));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseSwagger();

app.MapPost("v1/insert", async ([FromServices] BookService _bookService, [FromBody] BookModel oBookInsert) =>
 {

     oBookInsert.Id = ObjectId.GenerateNewId().ToString();
     await _bookService.CreateAsync(oBookInsert);
     return Results.Ok(oBookInsert);


 });

app.MapGet("v1/getdetail/{Id}", async ([FromServices] BookService _bookService, string Id) =>
{

    var oBook = await _bookService.GetAsync(Id);
    return Results.Ok(oBook);


});

app.MapPut("v1/update", async ([FromServices] BookService _bookService, [FromBody] BookModel oBookUpdate) =>
{   
    await _bookService.UpdateAsync(oBookUpdate.Id.ToString(), oBookUpdate);
    return Results.Ok(oBookUpdate);

});

app.MapDelete("v1/delete/{Id}", async ([FromServices] BookService _bookService, string Id) =>
{
    await _bookService.RemoveAsync(Id);
    return Results.Ok();


});


app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo Api v1");
    c.RoutePrefix = string.Empty;
});

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}