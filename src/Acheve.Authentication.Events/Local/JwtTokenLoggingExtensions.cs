using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace Acheve.Authentication.Events.Local
{
    /// <summary>
    ///     JwtBearer
    /// </summary>
    internal static class JwtTokenLoggingExtensions
    {
        private static readonly Action<ILogger, string, string, string, string, string, Exception> _challenge;
        private static readonly Action<ILogger, string, string, Exception> _messageReceived;
        private static readonly Action<ILogger, string, string, string, string, Exception> _tokenValidated;
        private static readonly Action<ILogger, string, string, string, Exception> _authenticationFailed;

        static JwtTokenLoggingExtensions()
        {
            _challenge = LoggerMessage.Define<string, string, string, string, string>(
                eventId: new EventId(100, "Challenge"),
                logLevel: LogLevel.Information,
                formatString: @"Scheme {scheme} - Event {event}
Description:
This event is raised before sending a response with a 401 status code to the client. There is no token or the token has expired or, may be, an unexpected exception has happened.
Relevant information:
AuthenticationFailure: {failure}
Error: {error}
ErrorDescription: {errorDescription}
Useful for:
Customize the 401 response. Call context.HandleResponse() to signal the handler that you are sending the response in this event to stop further proccesing.");
            
            _messageReceived = LoggerMessage.Define<string, string>(
                eventId: new EventId(101, "MessageReceived"),
                logLevel: LogLevel.Information,
                formatString: @"Scheme {scheme} - Event {event}
Description:
A request that must be authenticated has been received.
By default the handler finds the token in the Authentication header. You have the oportunity to find the token in other place and set it in the context.
You can also bypass all the handler processing and write custom logig to authenticate the request.
If an AuthenticationResult is generated in the MessageReceivedContext it is honored by the handler.
Useful for:
Get the token from a different location or adjust or reject the token based on custom logic: get the token and set the context.Token property.
Bypass the handler logic generating an AuthenticationResult: call context.Success(), context.Fail() or context.NoResult() based on your custom logic.");
            
            _tokenValidated = LoggerMessage.Define<string, string, string, string>(
                eventId: new EventId(101, "TokenValidated"),
                logLevel: LogLevel.Information,
                formatString: @"Scheme {scheme} - Event {event}
Description:
The token has been validated by, at least, one security token validator and a ClaimsPrincipal has been created.
You can also bypass all the handler processing and write custom logig to authenticate the request.
If an AuthenticationResult is generated in the TokenValidatedContext it is honored by the handler.
Relevant information:
User: {user}
Token: {token}
Useful for:
Add custom claims to the generated ClaimsPrincipal or replace it: use context.Principal.
Bypass the handler logic generating an AuthenticationResult: call context.Success(), context.Fail() or context.NoResult() based on your custom logic.");
            
            _authenticationFailed = LoggerMessage.Define<string, string, string>(
                eventId: new EventId(101, "AuthenticationFailed"),
                logLevel: LogLevel.Information,
                formatString: @"Scheme {scheme} - Event {event}
Description:
Any security token validator has validated the token but at least one of them has generated a validation failure exception.
In this event you can inspect the validation failure exceptions and decide if the request should be authenticated or not.
If an AuthenticationResult is generated in the AuthenticationFailedContext it is honored by the handler.
Relevant information:
Exception: {exception}
Useful for:
Add custom logic to handle an authentication failure. For example, manage the renew of public keys of the authority identity provider (can be done automatically through the RefreshOnIssuerKeyNotFound property).");
        }

        public static void Challenge(this ILogger logger, string scheme, Exception failure, string error, string errorDescription)
        {
            _challenge(
                logger,
                scheme,
                nameof(Challenge),
                failure?.ToString() ?? "N/A",
                error,
                errorDescription,
                null);
        }

        public static void MessageReceived(this ILogger logger, string scheme)
        {
            _messageReceived(
                logger,
                scheme,
                nameof(MessageReceived),
                null);
        }

        public static void TokenValidated(this ILogger logger, string scheme, ClaimsPrincipal principal, string token)
        {
            _tokenValidated(
                logger,
                scheme,
                nameof(TokenValidated),
                principal.ToFormattedLog(),
                token,
                null);
        }

        public static void AuthenticationFailed(this ILogger logger, string scheme, Exception exception)
        {
            _authenticationFailed(
                logger,
                scheme,
                nameof(AuthenticationFailed),
                exception?.ToString() ?? "N/A",
                null);
        }
    }
}
