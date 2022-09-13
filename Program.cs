using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using SomeApiController.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();

app.Run();   //run the app