using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Claims;
using System.Text.Json;

namespace Acheve.Authentication.Events.Remote.OpenIdConnect;

/// <summary>
///    OpenIdConnect
/// </summary>
internal static class OpenIdConnectLoggingExtensions
{
    private static readonly Action<ILogger, string, string, string, string, Exception> _redirectToIdentityProvider;
    private static readonly Action<ILogger, string, string, string, string, string, Exception> _messageReceived;
    private static readonly Action<ILogger, string, string, string, string, Exception> _tokenValidated;
    private static readonly Action<ILogger, string, string, string, string, Exception> _authorizationCodeReceived;
    private static readonly Action<ILogger, string, string, string, string, Exception> _tokenResponseReceived;
    private static readonly Action<ILogger, string, string, string, string, Exception> _userInformationReceived;
    private static readonly Action<ILogger, string, string, string, string, string, Exception> _redirectToIdentityProviderForSignOut;
    private static readonly Action<ILogger, string, string, string, Exception> _signedOutCallbackRedirect;
    private static readonly Action<ILogger, string, string, Exception> _remoteSignOut;
    private static readonly Action<ILogger, string, string, Exception> _authenticationFailed;

    static OpenIdConnectLoggingExtensions()
    {
        _redirectToIdentityProvider = LoggerMessage.Define<string, string, string, string>(
            eventId: new EventId(301, "RedirectToAuthorizationEndpoint"),
            logLevel: LogLevel.Information,
            formatString: @"Scheme {scheme} - Event {event}
Description:
This is a Remote Authentication Handler. You are going to be redirected to the OpenIdConnect server.
Relevant information:
ResponseType: {responseType}
RedirectUri: {redirectUri}
Useful for:
You can add information to the context.ProtocolMessage object (for example acr values or domain hint).
You can also bypass the handler and call context.HandleResponse() if you want to write the response by your own.");

        _messageReceived = LoggerMessage.Define<string, string, string, string, string>(
           eventId: new EventId(302, "MessageReceived"),
           logLevel: LogLevel.Information,
           formatString: @"Scheme {scheme} - Event {event}
Description:
Here we are managing the front channel response. Depending on the response_type we receive more or less information.
Relevant information:
id_token: {idToken}
code: {code}
access_token: {accessToken}
Useful for:
You can examine or add information to the context.ProtocolMessage object.
You can also bypass the handler and call context.HandleResponse() if you want to manage the message by your own.
Or skip further handler processing calling context.SkipHandler()
Or generate an AuthenticationResult calling context.Sucess(), context.Fail() or context.NoResult()");

        _tokenValidated = LoggerMessage.Define<string, string, string, string>(
           eventId: new EventId(303, "TokenValidated"),
           logLevel: LogLevel.Information,
           formatString: @"Scheme {scheme} - Event {event}
Description:
We are managing an Hybrid or Implicit flow. We have received an id_token and the handler has created a ClaimsPrincipal.
Relevant information:
id_token: {idToken}
Principal: {principal}
Useful for:
You can also bypass the handler and call context.HandleResponse() if you want to redeem the code by your own.
Or skip further handler processing calling context.SkipHandler()
Or generate an AuthenticationResult calling context.Sucess(), context.Fail() or context.NoResult()");

        _authorizationCodeReceived = LoggerMessage.Define<string, string, string, string>(
           eventId: new EventId(304, "AuthorizationCodeReceived"),
           logLevel: LogLevel.Information,
           formatString: @"Scheme {scheme} - Event {event}
Description:
We are managing a Code or Hybrid flow. We have received a code and we need to get an access_token from the back channel with it.
Relevant information:
code: {code}
Principal: {principal}
Useful for:
You can also bypass the handler and call context.HandleResponse() if you want to redeem the code by your own.
Or skip further handler processing calling context.SkipHandler()
Or generate an AuthenticationResult calling context.Sucess(), context.Fail() or context.NoResult()");

        _tokenResponseReceived = LoggerMessage.Define<string, string, string, string>(
           eventId: new EventId(305, "TokenResponseReceived"),
           logLevel: LogLevel.Information,
           formatString: @"Scheme {scheme} - Event {event}
Description:
We are managing an Code or Hybrid flow. After redeem the code, we have received an access_token.
Relevant information:
access_token: {accessToken}
Principal: {principal}
Useful for:
You can also bypass the handler and call context.HandleResponse() if you want to redeem the code by your own.
Or skip further handler processing calling context.SkipHandler()
Or generate an AuthenticationResult calling context.Sucess(), context.Fail() or context.NoResult()");

        _userInformationReceived = LoggerMessage.Define<string, string, string, string>(
          eventId: new EventId(306, "UserInformationReceived"),
          logLevel: LogLevel.Information,
          formatString: @"Scheme {scheme} - Event {event}
Description:
GetClaimsFromUserInfoEndpoint is true and we have received additional claims from the user-info endpoint.
Relevant information:
user: {user}
Principal: {principal}
Useful for:
You can also bypass the handler and call context.HandleResponse() if you want to redeem the code by your own.
Or skip further handler processing calling context.SkipHandler()
Or generate an AuthenticationResult calling context.Sucess(), context.Fail() or context.NoResult()");

        _redirectToIdentityProviderForSignOut = LoggerMessage.Define<string, string, string, string, string>(
          eventId: new EventId(307, "RedirectToIdentityProviderForSignOut"),
          logLevel: LogLevel.Information,
          formatString: @"Scheme {scheme} - Event {event}
Description:
SignOut has been called for this scheme and the handler is generating the redirect uri for the signout in the OpenId.Connect server.
Relevant information:
RedirectUri: {redirectUri}
CallbackPath: {callbackPath}
PostRedirectUri: {postRedirectUri}
Useful for:
You can also bypass the handler and call context.HandleResponse() if you want to redeem the code by your own.
Or skip further handler processing calling context.SkipHandler()
Or generate an AuthenticationResult calling context.Sucess(), context.Fail() or context.NoResult()");

        _signedOutCallbackRedirect = LoggerMessage.Define<string, string, string>(
                      eventId: new EventId(308, "SignedOutCallbackRedirect"),
                      logLevel: LogLevel.Information,
                      formatString: @"Scheme {scheme} - Event {event}
Description:
The signout has been performed in the OpenIdConnect.Server and the callback has been called.
Relevant information:
RedirectUri: {redirectUri}
Useful for:
You can also bypass the handler and call context.HandleResponse() if you want to redeem the code by your own.
Or skip further handler processing calling context.SkipHandler()
Or generate an AuthenticationResult calling context.Sucess(), context.Fail() or context.NoResult()");

        _remoteSignOut = LoggerMessage.Define<string, string>(
                      eventId: new EventId(309, "RemoteSignOut"),
                      logLevel: LogLevel.Information,
                      formatString: @"Scheme {scheme} - Event {event}
Description:
The signout has been performed in the OpenIdConnect.Server and the callback has been called.
Useful for:
You can also bypass the handler and call context.HandleResponse() if you want to redeem the code by your own.
Or skip further handler processing calling context.SkipHandler()
Or generate an AuthenticationResult calling context.Sucess(), context.Fail() or context.NoResult()");
        _authenticationFailed = LoggerMessage.Define<string, string>(
                      eventId: new EventId(310, "AuthenticationFailed"),
                      logLevel: LogLevel.Information,
                      formatString: @"Scheme {scheme} - Event {event}
Description:
The signout has been performed in the OpenIdConnect.Server and the callback has been called.
Useful for:
You can also bypass the handler and call context.HandleResponse() if you want to redeem the code by your own.
Or skip further handler processing calling context.SkipHandler()
Or generate an AuthenticationResult calling context.Sucess(), context.Fail() or context.NoResult()");
    }

    public static void RedirectToIdentityProvider(this ILogger logger, string scheme, OpenIdConnectMessage? protocolMessage)
    {
        _redirectToIdentityProvider(
            logger,
            scheme,
            nameof(RedirectToIdentityProvider),
            protocolMessage?.ResponseType ?? "N/A",
            protocolMessage?.CreateAuthenticationRequestUrl() ?? "N/A",
            null);
    }

    public static void MessageReceived(this ILogger logger, string scheme, OpenIdConnectMessage? protocolMessage)
    {
        _messageReceived(
            logger,
            scheme,
            nameof(MessageReceived),
            protocolMessage?.IdToken ?? "N/A",
            protocolMessage?.Code ?? "N/A",
            protocolMessage?.AccessToken ?? "N/A",
            null);
    }

    public static void TokenValidated(this ILogger logger, string scheme, OpenIdConnectMessage? protocolMessage, ClaimsPrincipal? principal)
    {
        _tokenValidated(
            logger,
            scheme,
            nameof(TokenValidated),
            protocolMessage?.IdToken ?? "N/A",
            principal?.ToFormattedLog() ?? "N/A",
            null);
    }

    public static void AuthorizationCodeReceived(this ILogger logger, string scheme, OpenIdConnectMessage? protocolMessage, ClaimsPrincipal? principal)
    {
        _authorizationCodeReceived(
            logger,
            scheme,
            nameof(AuthorizationCodeReceived),
            protocolMessage?.Code ?? "N/A",
            principal?.ToFormattedLog() ?? "N/A",
            null);
    }

    public static void TokenResponseReceived(this ILogger logger, string scheme, OpenIdConnectMessage? protocolMessage, OpenIdConnectMessage? tokenEndpointResponse, ClaimsPrincipal? principal)
    {
        _tokenResponseReceived(
            logger,
            scheme,
            nameof(TokenResponseReceived),
            tokenEndpointResponse?.AccessToken ?? "N/A",
            principal?.ToFormattedLog() ?? "N/A",
            null);
    }

    public static void UserInformationReceived(this ILogger logger, string scheme, OpenIdConnectMessage? protocolMessage, ClaimsPrincipal? principal, JsonDocument? user)
    {
        _userInformationReceived(
            logger,
            scheme,
            nameof(UserInformationReceived),
            user?.RootElement.GetRawText() ?? "N/A",
            principal?.ToFormattedLog() ?? "N/A",
            null);
    }

    public static void RedirectToIdentityProviderForSignOut(this ILogger logger, string scheme, OpenIdConnectMessage? protocolMessage, OpenIdConnectOptions? options, AuthenticationProperties? properties)
    {
        _redirectToIdentityProviderForSignOut(
            logger,
            scheme,
            nameof(RedirectToIdentityProviderForSignOut),
            protocolMessage?.CreateLogoutRequestUrl() ?? "N/A",
            options?.SignedOutCallbackPath.ToString() ?? "N/A",
            properties?.RedirectUri ?? "N/A",
            null);
    }

    public static void SignedOutCallbackRedirect(this ILogger logger, string scheme, AuthenticationProperties? properties)
    {
        _signedOutCallbackRedirect(
            logger,
            scheme,
            nameof(SignedOutCallbackRedirect),
            properties?.RedirectUri ?? "N/A",
            null);
    }

    public static void RemoteSignOut(this ILogger logger, string scheme, OpenIdConnectMessage? protocolMessage)
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
