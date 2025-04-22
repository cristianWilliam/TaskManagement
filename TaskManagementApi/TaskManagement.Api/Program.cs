using System.Text.Json.Serialization;
using TaskManagement.Api.Cards.Hubs;
using TaskManagement.Api.Extensions;
using TaskManagement.Application;
using TaskManagement.Infra;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Layers
builder.Services.AddInfraLayer(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddPresentationLayer();
builder.Services.AddSignalR();

// Enable cors
builder.Services.AddCors(policy =>
{
    policy.AddDefaultPolicy(policyBuilder => { policyBuilder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<CardsHub>("api/cards/hub");
app.UseCors();

// app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();