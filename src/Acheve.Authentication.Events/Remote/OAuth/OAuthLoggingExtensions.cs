using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace Acheve.Authentication.Events.Remote.OAuth;

/// <summary>
///    OAuth (OAuth, Twitter, Google, Facebook and MicrosoftAccount)
/// </summary>
internal static class OAuthLoggingExtensions
{
    private static readonly Action<ILogger, string, string, string, Exception> _redirectToAuthorizationEndpoint;
    private static readonly Action<ILogger, string, string, string, string, Exception> _creatingTicket;

    static OAuthLoggingExtensions()
    {
        _redirectToAuthorizationEndpoint = LoggerMessage.Define<string, string, string>(
            eventId: new EventId(1, "RedirectToAuthorizationEndpoint"),
            logLevel: LogLevel.Information,
            formatString: @"Scheme {scheme} - Event {event}
Description:
This is a Remote Authentication Handler. You are going to be redirected to the Identity provider.
Relevant information:
RedirectUri: {redirectUri}
Useful for:
This is your last chance to customize the redirect url before being redirected.");

        _creatingTicket = LoggerMessage.Define<string, string, string, string>(
            eventId: new EventId(2, "CreatingTicket"),
            logLevel: LogLevel.Information,
            formatString: @"Scheme {scheme} - Event {event}
Description:
The identity provider has authenticated the user, redirected to the application and the handler has created the ClaimsPrincipal.
Relevant information:
You can review the principal created and the information the identity provider has sent.
AccessToken: {accessToken}
Principal: {principal}
Useful for:
You can add some custom information to the ClaimsPrincipal or implement additional verification code to decide if the user is authenticated or not.");
    }

    public static void RedirectToAuthorizationEndpoint(this ILogger logger, string scheme, string? redirectUri)
    {
        _redirectToAuthorizationEndpoint(
            logger,
            scheme,
            nameof(RedirectToAuthorizationEndpoint),
            redirectUri ?? "N/A",
            null);
    }

    public static void CreatingTicket(this ILogger logger, string scheme, string? accessToken, ClaimsPrincipal? principal)
    {
        _creatingTicket(
            logger,
            scheme,
            nameof(CreatingTicket),
            accessToken ?? "N/A",
            principal?.ToFormattedLog() ?? "N/A",
            null);
    }
}
