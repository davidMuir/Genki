[![Build Status](https://travis-ci.org/davidMuir/Genki.svg?branch=master)](https://travis-ci.org/davidMuir/Genki)

# Genki

Provides a way to define healthchecks for a .Net Core MVC app

## Defining a Health Check Step

```csharp

public class MyStep : IHealthCheckStep
{
    private readonly IMyAsyncService _myAsyncService;

    // Genki will plug into the default DI container so you
    // can bring in your services
    public MyStep(IMyAsyncService myAsyncService)
    {
        _myAsyncService = myAsyncService;
    }

    public string Name => "My Check";
    
    public string Description => "Checks stuff";

    public Importance Importance => Importance.Normal;

    public async Task<bool> GetIsHealthyAsync()
    {
        try
        {
            await _myAsyncService.GetValueAsync();

            return true;
        }
        catch(Exception)
        {
            return false;
        }
    }
}

```

## Wiring everything up

```csharp

public void ConfigureServices(IServiceCollection services)
{
    services
        .AddGenki(options =>
        {
            options.ServiceName = "My Web Service";
            options.Endpoint = "/genki";
        })
        .AddStep<MyStep>();
}

public void Configure(IApplicationBuilder app)
{
    app.UseGenki();

    app.Run(async (context) =>
    {
        await context.Response.WriteAsync("Hello World!");
    });
}

```

Then, after starting up our service we can access our endpoint in any way we 
like and get a response like the following

```json

{
  "service": "My Web Service",
  "isHealthy": true,
  "steps": [
    {
      "name": "My Check",
      "description": "Checks stuff",
      "importance": "normal",
      "isHealthy": true
    }
  ]
}

```
