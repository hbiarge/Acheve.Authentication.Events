using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace Acheve.Authentication.Events.Local;

/// <summary>
///     Cookies
/// </summary>
internal static class CookiesLoggingExtensions
{
    internal static readonly Action<ILogger, string, string, string, Exception> _validatePrincipal;
    internal static readonly Action<ILogger, string, string, string, Exception> _signingIn;
    internal static readonly Action<ILogger, string, string, string, Exception> _signedIn;
    internal static readonly Action<ILogger, string, string, Exception> _signingOut;
    internal static readonly Action<ILogger, string, string, string, Exception> _redirectToLogin;
    internal static readonly Action<ILogger, string, string, string, Exception> _redirectToReturnUrl;
    internal static readonly Action<ILogger, string, string, string, Exception> _redirectToAccessDenied;
    internal static readonly Action<ILogger, string, string, string, Exception> _redirectToLogout;

    static CookiesLoggingExtensions()
    {
        _validatePrincipal = LoggerMessage.Define<string, string, string>(
            eventId: new EventId(200, "ValidatePrincipal"),
            logLevel: LogLevel.Information,
            formatString: @"Scheme {scheme} - Event {event}
Description:
The cookie has been read and validated and a ClaimsPrincipal has been created.
You can replace the principal with a new one or reject the principal based on custom logic.
You can also force the renewal of the cookie setting context.ShouldRenew to true.
This method is called in every request.
Relevant information:
Principal: {principal}
Useful for:
Perform custom logic in the incoming ClaimsPrincipal or force the cookie renewal.");

        _signingIn = LoggerMessage.Define<string, string, string>(
            eventId: new EventId(201, "SigningIn"),
            logLevel: LogLevel.Information,
            formatString: @"Scheme {scheme} - Event {event}
Description:
The handler is configuring the settings to send the cookie to the client.
You can customize the cookie settings in this event.
This method is called once per SigIn process in the scheme.
Relevant information:
Principal: {principal}
Useful for:
Customize the cookie options.");

        _signedIn = LoggerMessage.Define<string, string, string>(
            eventId: new EventId(202, "SignedIn"),
            logLevel: LogLevel.Information,
            formatString: @"Scheme {scheme} - Event {event}
Description:
The authentication cookie has been generated and appended to the response.
You can customize the cookie settings in this event.
This method is called once when there is a SigIn in this scheme.
Relevant information:
Principal: {principal}
Useful for:
Track sigins?.");

        _signingOut = LoggerMessage.Define<string, string>(
            eventId: new EventId(202, "SigningOut"),
            logLevel: LogLevel.Information,
            formatString: @"Scheme {scheme} - Event {event}
Description:
This event is called before deleting the authentication cookie.
This method is called once when there is a SigOut in this scheme.
Useful for:
Track sigouts?.");
        _redirectToLogin = LoggerMessage.Define<string, string, string>(
            eventId: new EventId(210, "RedirectToLogin"),
            logLevel: LogLevel.Information,
            formatString: @"Scheme {scheme} - Event {event}
Description:
This event is called when the handler is handling a challenge with this scheme.
You can validate and customize the redirect uri.
This method is called once when there is a Challenge in this scheme.
Relevant information:
RedirectUri: {redirectUri}");
        _redirectToReturnUrl = LoggerMessage.Define<string, string, string>(
            eventId: new EventId(210, "RedirectToReturnUrl"),
            logLevel: LogLevel.Information,
            formatString: @"Scheme {scheme} - Event {event}
Description:
This event is called before redirecting to the return url specified in a signin or signout process for this scheme if a returnUrl is present..
You can validate and customize the redirect uri.
Relevant information:
RedirectUri: {redirectUri}");
        _redirectToAccessDenied = LoggerMessage.Define<string, string, string>(
            eventId: new EventId(210, "RedirectToAccessDenied"),
            logLevel: LogLevel.Information,
            formatString: @"Scheme {scheme} - Event {event}
Description:
This event is called when the handler is handling a forbidden with this scheme.
You can validate and customize the redirect uri.
This method is called once when there is a Forbidden in this scheme.
Relevant information:
RedirectUri: {redirectUri}");
        _redirectToLogout = LoggerMessage.Define<string, string, string>(
            eventId: new EventId(213, "RedirectToLogout"),
            logLevel: LogLevel.Information,
            formatString: @"Scheme {scheme} - Event {event}
Description:
Aparently this event is not called anymore in the handler.
Relevant information:
RedirectUri: {redirectUri}");
    }

}

internal static class CookiesLoggingExtensionMethods
{
    public static void ValidatePrincipal(this ILogger logger, string scheme, ClaimsPrincipal? principal)
    {
        CookiesLoggingExtensions._validatePrincipal(
            logger,
            scheme,
            nameof(ValidatePrincipal),
            principal?.ToFormattedLog() ?? "N/A",
            null);
    }

    public static void SigningIn(this ILogger logger, string scheme, ClaimsPrincipal? principal)
    {
        CookiesLoggingExtensions._signingIn(
            logger,
            scheme,
            nameof(SigningIn),
            principal?.ToFormattedLog() ?? "N/A",
            null);
    }

    public static void SignedIn(this ILogger logger, string scheme, ClaimsPrincipal? principal)
    {
        CookiesLoggingExtensions._signedIn(
            logger,
            scheme,
            nameof(SignedIn),
            principal?.ToFormattedLog() ?? "N/A",
            null);
    }

    public static void SigningOut(this ILogger logger, string scheme)
    {
        CookiesLoggingExtensions._signingOut(
            logger,
            scheme,
            nameof(SigningOut),
            null);
    }

    public static void RedirectToLogin(this ILogger logger, string scheme, string? redirectUri)
    {
        CookiesLoggingExtensions._redirectToLogin(
            logger,
            scheme,
            nameof(RedirectToLogin),
            redirectUri ?? "N/A",
            null);
    }

    public static void RedirectToReturnUrl(this ILogger logger, string scheme, string? redirectUri)
    {
        CookiesLoggingExtensions._redirectToReturnUrl(
            logger,
            scheme,
            nameof(RedirectToReturnUrl),
            redirectUri ?? "N/A",
            null);
    }

    public static void RedirectToAccessDenied(this ILogger logger, string scheme, string? redirectUri)
    {
        CookiesLoggingExtensions._redirectToAccessDenied(
            logger,
            scheme,
            nameof(RedirectToAccessDenied),
            redirectUri ?? "N/A",
            null);
    }

    public static void RedirectToLogout(this ILogger logger, string scheme, string? redirectUri)
    {
        CookiesLoggingExtensions._redirectToLogout(
            logger,
            scheme,
            nameof(RedirectToLogout),
            redirectUri ?? "N/A",
            null);
    }
}
