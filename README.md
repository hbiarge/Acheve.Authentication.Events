# Acheve.Authentication.Events
Authentication events that logs every authentication event for an authentication handler in the Asp.Net Core with useful information to help understand what´s happening in the authentication process.

![sample output](https://pbs.twimg.com/media/D1h_6xlWoAAO1u_.jpg:large)

Just register your authentication handlers in your ```Startup``` class and configure the ```options.EventsType``` with the one is using your authentication handler.
As the events implementations use an injected ILogger, we need to register the type also in the DI container.
And that´s it!

```csharp

            services.AddAuthentication()
                .AddTwitter(options =>
                {
                    Configuration.Bind("twitter", options);

                    options.EventsType = typeof(LogTwitterEvents);
                });
            services.AddTransient<LogTwitterEvents>();

```

It is recomended to filter the log events to send only the ones that relates with the authentication process. This is the configuration I use to use (with serilog)

```csharp

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    theme: AnsiConsoleTheme.Literate)
                .CreateLogger();

```