using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrimeWeb01.Middleware;

namespace PrimeWeb01.UnitTests;

public class PrimeWeb_CheckPrimeShould : IDisposable
{
    private readonly IHost _host;
    private readonly HttpClient _client;

    public PrimeWeb_CheckPrimeShould()
    {
        _host = new HostBuilder()
            .ConfigureWebHost(web => web.UseTestServer().UseStartup<PrimeWebTestStartup>())
            .Build();
        _host.Start();
        _client = _host.GetTestClient();
    }

    [Theory]
    [InlineData("/checkprime?5", "5 is prime!")]
    [InlineData("/checkprime?6", "6 is NOT prime!")]
    [InlineData("/checkprime?-7", "-7 is NOT prime!")]
    [InlineData("/checkprime?0", "0 is NOT prime!")]
    [InlineData("/checkprime?1", "1 is NOT prime!")]
    [InlineData("/checkprime?2", "2 is prime!")]
    [InlineData("/checkprime?2147483647", "2147483647 is prime!")]
    [InlineData("/checkprime?-2147483648", "-2147483648 is NOT prime!")]
    [InlineData("/CHECKPRIME?5", "5 is prime!")]
    public async Task ReturnClassification(string path, string expected)
    {
        await AssertResponse(_client, path, expected);
    }

    [Theory]
    [InlineData("/checkprime")]
    [InlineData("/checkprime?")]
    [InlineData("/checkprime?abc")]
    [InlineData("/checkprime?5.5")]
    [InlineData("/checkprime?number=5")]
    [InlineData("/checkprime?5&6")]
    [InlineData("/checkprime?5?")]
    [InlineData("/checkprime?2147483648")]
    [InlineData("/checkprime?-2147483649")]
    public async Task ReturnInstructionsForInvalidInput(string path)
    {
        await AssertResponse(_client, path, "Pass in a number to check in the form /checkprime?5");
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/unknown")]
    [InlineData("/weatherforecast")]
    [InlineData("/checkprime/?5")]
    [InlineData("/prefix/checkprime?5")]
    public async Task ReturnGreetingForOtherPaths(string path)
    {
        await AssertResponse(_client, path,
            "Hello World! To check if a number is prime, provide URL of the form /checkprime?5");
    }

    [Theory]
    [InlineData("/isprime?5", "5 is prime!")]
    [InlineData("/isprime?6", "6 is NOT prime!")]
    [InlineData("/isprime", "Pass in a number to check in the form /isprime?5")]
    [InlineData("/checkprime?5",
        "Hello World! To check if a number is prime, provide URL of the form /isprime?5")]
    public async Task HonorConfiguredPath(string path, string expected)
    {
        using var host = new HostBuilder()
            .ConfigureWebHost(web => web
                .UseTestServer()
                .ConfigureServices(services => services.AddTransient<PrimeService>())
                .Configure(app => app.UsePrimeChecker("/isprime")))
            .Build();
        await host.StartAsync();
        using var client = host.GetTestClient();

        await AssertResponse(client, path, expected);
        await host.StopAsync();
    }

    private static async Task AssertResponse(HttpClient client, string path, string expected)
    {
        using var response = await client.GetAsync(path);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("text/plain", response.Content.Headers.ContentType?.MediaType);
        Assert.Equal(expected, await response.Content.ReadAsStringAsync());
    }

    public void Dispose()
    {
        _client.Dispose();
        _host.Dispose();
    }
}

public class PrimeWebTestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<PrimeService>();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UsePrimeChecker();
    }
}
