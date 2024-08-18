using Polly;
using Polly.Simmy;
using ToDoApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Chaos Engg Config Start
var httpClientBuilder = builder.Services.AddHttpClient<ITodosClient, TodosClient>(client =>
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com"));
httpClientBuilder.AddResilienceHandler("chaos", pipelineBuilder =>
{
    // Set the chaos injection rate to 50%
    const double injectionRate = 0.5;

    pipelineBuilder
        .AddChaosLatency(injectionRate, TimeSpan.FromSeconds(5)) // Add latency to simulate network delays
        .AddChaosFault(injectionRate, () => new InvalidOperationException("Chaos strategy injection!")) // Inject faults to simulate system errors
        .AddChaosOutcome(injectionRate, () => new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)); // Simulate server errors
});
// Chaos Engg Config End


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
