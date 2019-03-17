using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Claims;

namespace Acheve.Authentication.Events.Remote.OpenIdConnect
{
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
Information:
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
Information:
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
We are managing an Hybrid or implicit flow. We have received an id_token and the handler has created a ClaimsPrincipal.
Information:
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
Information:
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
Information:
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
Information:
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
Information:
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
Information:
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

        public static void RedirectToIdentityProvider(this ILogger logger, string scheme, OpenIdConnectMessage protocolMessage)
        {
            _redirectToIdentityProvider(
                logger,
                scheme,
                nameof(RedirectToIdentityProvider),
                protocolMessage.ResponseType,
                protocolMessage.CreateAuthenticationRequestUrl(),
                null);
        }

        public static void MessageReceived(this ILogger logger, string scheme, OpenIdConnectMessage protocolMessage)
        {
            _messageReceived(
                logger,
                scheme,
                nameof(MessageReceived),
                protocolMessage.IdToken ?? "N/A",
                protocolMessage.Code ?? "N/A",
                protocolMessage.AccessToken ?? "N/A",
                null);
        }

        public static void TokenValidated(this ILogger logger, string scheme, OpenIdConnectMessage protocolMessage, ClaimsPrincipal principal)
        {
            _tokenValidated(
                logger,
                scheme,
                nameof(TokenValidated),
                protocolMessage.IdToken,
                principal.ToFormattedLog(),
                null);
        }

        public static void AuthorizationCodeReceived(this ILogger logger, string scheme, OpenIdConnectMessage protocolMessage, ClaimsPrincipal principal)
        {
            _authorizationCodeReceived(
                logger,
                scheme,
                nameof(AuthorizationCodeReceived),
                protocolMessage.Code,
                principal.ToFormattedLog(),
                null);
        }

        public static void TokenResponseReceived(this ILogger logger, string scheme, OpenIdConnectMessage protocolMessage, OpenIdConnectMessage tokenEndpointResponse, ClaimsPrincipal principal)
        {
            _tokenResponseReceived(
                logger,
                scheme,
                nameof(TokenResponseReceived),
                tokenEndpointResponse.AccessToken,
                principal.ToFormattedLog(),
                null);
        }

        public static void UserInformationReceived(this ILogger logger, string scheme, OpenIdConnectMessage protocolMessage, ClaimsPrincipal principal, JObject user)
        {
            _userInformationReceived(
                logger,
                scheme,
                nameof(UserInformationReceived),
                user.ToString(),
                principal.ToFormattedLog(),
                null);
        }

        public static void RedirectToIdentityProviderForSignOut(this ILogger logger, string scheme, OpenIdConnectMessage protocolMessage, OpenIdConnectOptions options, AuthenticationProperties properties)
        {
            _redirectToIdentityProviderForSignOut(
                logger,
                scheme,
                nameof(RedirectToIdentityProviderForSignOut),
                protocolMessage.CreateLogoutRequestUrl(),
                options.SignedOutCallbackPath,
                properties.RedirectUri ?? "N/A",
                null);
        }

        public static void SignedOutCallbackRedirect(this ILogger logger, string scheme, AuthenticationProperties properties)
        {
            _signedOutCallbackRedirect(
                logger,
                scheme,
                nameof(SignedOutCallbackRedirect),
                properties.RedirectUri ?? "N/A",
                null);
        }

        public static void RemoteSignOut(this ILogger logger, string scheme, OpenIdConnectMessage protocolMessage)
        {
            _remoteSignOut(
                logger,
                scheme,
                nameof(RemoteSignOut),
                null);
        }

        public static void AuthenticationFailed(this ILogger logger, string scheme, Exception exception)
        {
            _authenticationFailed(
                logger,
                scheme,
                nameof(AuthenticationFailed),
                null);
        }
    }
}