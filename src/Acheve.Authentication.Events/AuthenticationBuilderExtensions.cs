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
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Authentication
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder UseLogEvents(this AuthenticationBuilder builder)
        {
            builder.Services.AddTransient<LogCookieAuthenticationEvents>();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<CookieAuthenticationOptions>, EventsTypeCookieAuthenticationPostConfigureOptions>());
            builder.Services.AddTransient<LogJwtBearerEvents>();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>, EventsTypeJwtBearerPostConfigureOptions>());

            builder.Services.AddTransient<LogOAuthEvents>();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<OAuthOptions>, EventsTypeOAuthPostConfigureOptions>());
            builder.Services.AddTransient<LogFacebookEvents>();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<FacebookOptions>, EventsTypeFacebookPostConfigureOptions>());
            builder.Services.AddTransient<LogGoogleEvents>();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<GoogleOptions>, EventsTypeGooglePostConfigureOptions>());
            builder.Services.AddTransient<LogMicrosoftAccountEvents>();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<MicrosoftAccountOptions>, EventsTypeMicrosoftAccountPostConfigureOptions>());
            builder.Services.AddTransient<LogTwitterEvents>();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<TwitterOptions>, EventsTypeTwitterPostConfigureOptions>());

            builder.Services.AddTransient<LogOpenIdConnectEvents>();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<OpenIdConnectOptions>, EventsTypeOpenIdConnectPostConfigureOptions>());
            builder.Services.AddTransient<LogWsFederationEvents>();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<WsFederationOptions>, EventsTypeWsFederationPostConfigureOptions>());

            return builder;
        }
    }
}
