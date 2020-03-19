# Acheve.Authentication.Events
Authentication events that logs every authentication event for an authentication handler in the Asp.Net Core with useful information that helps to understand what´s happening in the authentication process.

![sample output](https://pbs.twimg.com/media/D1h_6xlWoAAO1u_.jpg:large)

Just register your authentication handlers in your ```Startup``` class and add the ```UseLogEvents``` extension method.
And that´s it!

```csharp

    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .UseLogEvents()
    .AddCookie(options =>
    {
        options.Events.OnValidatePrincipal = context => Task.CompletedTask;
    })
    .AddOpenIdConnect(options =>
    {
        options.Authority = "https://demo.identityserver.io";
        options.ClientId = "interactive.public";

        options.ResponseType = OpenIdConnectResponseType.Code;
        options.UsePkce = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.SaveTokens = true;

        options.Events.OnRedirectToIdentityProvider = context => Task.CompletedTask;

        options.TokenValidationParameters.NameClaimType = "name";
        options.TokenValidationParameters.RoleClaimType = "role";

        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.SignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    });

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