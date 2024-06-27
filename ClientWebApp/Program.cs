using ClientWebApp.Services;
using StackExchange.Redis;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var redisConnectionString = args[0];
ConnectionMultiplexer redis = await ConnectionMultiplexer.ConnectAsync(redisConnectionString);

builder.Services.AddScoped((sp) => redis.GetDatabase());

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();


var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", TodoService.GetTodosAsync);
todosApi.MapPost("/", TodoService.CreateTodo);
todosApi.MapDelete("/", TodoService.ClearTodo);

app.Run();


[JsonSerializable(typeof(string[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}