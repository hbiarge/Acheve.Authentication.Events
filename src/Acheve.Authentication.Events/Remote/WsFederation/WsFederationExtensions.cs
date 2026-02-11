using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.WsFederation;
using System;
using System.Security.Claims;

namespace Acheve.Authentication.Events.Remote.WsFederation;

/// <summary>
///    WsFederation
/// </summary>
internal static class WsFederationExtensions
{
    private static readonly Action<ILogger, string, string, string, Exception> _redirectToIdentityProvider;
    private static readonly Action<ILogger, string, string, Exception> _messageReceived;
    private static readonly Action<ILogger, string, string, Exception> _securityTokenReceived;
    private static readonly Action<ILogger, string, string, Exception> _securityTokenValidated;
    private static readonly Action<ILogger, string, string, Exception> _remoteSignOut;
    private static readonly Action<ILogger, string, string, Exception> _authenticationFailed;

    static WsFederationExtensions()
    {
        _redirectToIdentityProvider = LoggerMessage.Define<string, string, string>(
            eventId: new EventId(401, "RedirectToIdentityProvider"),
            logLevel: LogLevel.Information,
            formatString: @"Scheme {scheme} - Event {event}
Description:
This is a Remote Authentication Handler. You are going to be redirected to the OpenIdConnect server.
Relevant information:
RedirectUri: {redirectUri}
Useful for:
You can add information to the context.ProtocolMessage object (for example acr values or domain hint).
You can also bypass the handler and call context.HandleResponse() if you want to write the response by your own.");

        _messageReceived = LoggerMessage.Define<string, string>(
           eventId: new EventId(402, "MessageReceived"),
           logLevel: LogLevel.Information,
           formatString: @"Scheme {scheme} - Event {event}
Description:
Here we are managing the front channel response. Depending on the response_type we receive more or less information.
Useful for:
You can examine or add information to the context.ProtocolMessage object.
You can also bypass the handler and call context.HandleResponse() if you want to manage the message by your own.
Or skip further handler processing calling context.SkipHandler()
Or generate an AuthenticationResult calling context.Sucess(), context.Fail() or context.NoResult()");

        _securityTokenReceived = LoggerMessage.Define<string, string>(
           eventId: new EventId(403, "SecurityTokenReceived"),
           logLevel: LogLevel.Information,
           formatString: @"Scheme {scheme} - Event {event}
Description:
We are managing an Code or Hybrid flow. After redeem the code, we have received an access_token.
Useful for:
You can also bypass the handler and call context.HandleResponse() if you want to redeem the code by your own.
Or skip further handler processing calling context.SkipHandler()
Or generate an AuthenticationResult calling context.Sucess(), context.Fail() or context.NoResult()");

        _securityTokenValidated = LoggerMessage.Define<string, string>(
           eventId: new EventId(404, "SecurityTokenValidated"),
           logLevel: LogLevel.Information,
           formatString: @"Scheme {scheme} - Event {event}
Description:
We are managing an Hybrid or implicit flow. We have received an id_token and the handler has created a ClaimsPrincipal.
Useful for:
You can also bypass the handler and call context.HandleResponse() if you want to redeem the code by your own.
Or skip further handler processing calling context.SkipHandler()
Or generate an AuthenticationResult calling context.Sucess(), context.Fail() or context.NoResult()");

        _remoteSignOut = LoggerMessage.Define<string, string>(
          eventId: new EventId(405, "RemoteSignOut"),
          logLevel: LogLevel.Information,
          formatString: @"Scheme {scheme} - Event {event}
Description:
The signout has been performed in the OpenIdConnect.Server and the callback has been called.
Useful for:
You can also bypass the handler and call context.HandleResponse() if you want to redeem the code by your own.
Or skip further handler processing calling context.SkipHandler()
Or generate an AuthenticationResult calling context.Sucess(), context.Fail() or context.NoResult()");

        _authenticationFailed = LoggerMessage.Define<string, string>(
                      eventId: new EventId(406, "AuthenticationFailed"),
                      logLevel: LogLevel.Information,
                      formatString: @"Scheme {scheme} - Event {event}
Description:
The signout has been performed in the OpenIdConnect.Server and the callback has been called.
Useful for:
You can also bypass the handler and call context.HandleResponse() if you want to redeem the code by your own.
Or skip further handler processing calling context.SkipHandler()
Or generate an AuthenticationResult calling context.Sucess(), context.Fail() or context.NoResult()");
    }

    public static void RedirectToIdentityProvider(
        this ILogger logger,
        string scheme,
        WsFederationMessage? protocolMessage)
    {
        _redirectToIdentityProvider(
            logger,
            scheme,
            nameof(RedirectToIdentityProvider),
            protocolMessage?.CreateSignInUrl() ?? "N/A",
            null);
    }

    public static void MessageReceived(
        this ILogger logger,
        string scheme,
        WsFederationMessage? protocolMessage)
    {
        _messageReceived(
            logger,
            scheme,
            nameof(MessageReceived),
            null);
    }

    public static void SecurityTokenReceived(
        this ILogger logger,
        string scheme,
        WsFederationMessage? protocolMessage,
        ClaimsPrincipal? principal)
    {
        _securityTokenReceived(
            logger,
            scheme,
            nameof(SecurityTokenReceived),
            null);
    }

    public static void SecurityTokenValidated(
        this ILogger logger,
        string scheme,
        WsFederationMessage? protocolMessage,
        ClaimsPrincipal? principal)
    {
        _securityTokenValidated(
            logger,
            scheme,
            nameof(SecurityTokenValidated),
            null);
    }

    public static void RemoteSignOut(
        this ILogger logger,
        string scheme,
        WsFederationMessage? protocolMessage)
    {
        _remoteSignOut(
            logger,
            scheme,
            nameof(RemoteSignOut),
            null);
    }

    public static void AuthenticationFailed(this ILogger logger, string scheme, Exception? exception)
    {
        _authenticationFailed(
            logger,
            scheme,
            nameof(AuthenticationFailed),
            null);
    }
}
