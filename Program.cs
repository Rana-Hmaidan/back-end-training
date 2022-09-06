using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);

//creates an HTTP GET endpoint / which returns Hello World!
app.MapGet("/", () => "Hello World!");
//Returns a JSON with your name when you ask to GET the /whoami resource/route on it
app.MapGet("/whoami", () => Results.Json(new { Name = "Rana Hmaidan"}));

app.Run();   //run the app
