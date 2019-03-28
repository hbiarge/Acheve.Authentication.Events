using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Authentication
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder UseLogEvents(this AuthenticationBuilder builder)
        {
            builder.Services.AddTransient<LogCookieAuthenticationEvents>();
            builder.Services.AddSingleton<EventsTypeCookieAuthenticationPostConfigureOptions>();
            builder.Services.AddTransient<LogJwtBearerEvents>();
            builder.Services.AddSingleton<EventsTypeJwtBearerPostConfigureOptions>();

            builder.Services.AddTransient<LogOAuthEvents>();
            builder.Services.AddSingleton<EventsTypeOAuthPostConfigureOptions>();
            builder.Services.AddTransient<LogFacebookEvents>();
            builder.Services.AddSingleton<EventsTypeFacebookPostConfigureOptions>();
            builder.Services.AddTransient<LogGoogleEvents>();
            builder.Services.AddSingleton<EventsTypeGooglePostConfigureOptions>();
            builder.Services.AddTransient<LogMicrosoftAccountEvents>();
            builder.Services.AddSingleton<EventsTypeMicrosoftAccountPostConfigureOptions>();
            builder.Services.AddTransient<LogTwitterEvents>();
            builder.Services.AddSingleton<EventsTypeTwitterPostConfigureOptions>();

            builder.Services.AddTransient<LogOpenIdConnectEvents>();
            builder.Services.AddSingleton<EventsTypeOpenIdConnectPostConfigureOptions>();
            builder.Services.AddTransient<LogWsFederationEvents>();
            builder.Services.AddSingleton<EventsTypeWsFederationPostConfigureOptions>();

            return builder;
        }
    }
}
