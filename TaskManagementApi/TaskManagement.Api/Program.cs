using System.Text.Json.Serialization;
using TaskManagement.Api.Extensions;
using TaskManagement.Application;
using TaskManagement.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Layers
builder.Services.AddPersistenceLayer(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddPresentationLayer();

// Enable cors
builder.Services.AddCors(policy =>
{
    policy.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().SetIsOriginAllowed(_ => true);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.MapWebSocketEndpoint();

// app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class ApiProgram
{
};