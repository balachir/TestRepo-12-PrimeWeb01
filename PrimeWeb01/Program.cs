using PrimeWeb01;
using PrimeWeb01.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<PrimeService>();

var app = builder.Build();

app.UsePrimeChecker();

app.Run();
