using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.AspNetCore.Authentication
{
    /// <summary>
    ///    OAuth (OAuth, Twitter, Google, Facebook and MicrosoftAccount)
    /// </summary>
    internal static class OAuthLoggingExtensions
    {
        private static readonly Action<ILogger, string, string, string, Exception> _redirectToAuthorizationEndpoint;
        private static readonly Action<ILogger, string, string, string, Exception> _creatingTicket;
        private static readonly Action<ILogger, string, string, string, Exception> _ticketReceived;
        private static readonly Action<ILogger, string, string, Exception> _remoteFailure;

        static OAuthLoggingExtensions()
        {

            _redirectToAuthorizationEndpoint = LoggerMessage.Define<string, string, string>(
                eventId: new EventId(1, "RedirectToAuthorizationEndpoint"),
                logLevel: LogLevel.Information,
                formatString: @"Scheme {scheme} - Handler {handler} - Event {event}
Description:
This is a Remote Authentication Handler. You are going to be redirected to the Identity provider.
Information:
RedirectUri: {redirectUri}
Useful for:
This is your last chance to customize the redirect url before being redirected.");
            _creatingTicket = LoggerMessage.Define<string, string, string>(
                eventId: new EventId(2, "CreatingTicket"),
                logLevel: LogLevel.Information,
                formatString: @"Scheme {scheme} - Handler {handler} - Event {event}
Description:
The identity provider has authenticated the user, redirected to the application and the handler has created the ClaimsPrincipal.
Information:
You can review the principal created and the information the identity provider has sent.
AccessToken: {accessToken}
Useful for:
You can add some custom information to the ClaimsPrincipal or implement additional verification code to decide if the user is authenticated or not.");
            _ticketReceived = LoggerMessage.Define<string, string, string>(
                eventId: new EventId(3, "TicketReceived"),
                logLevel: LogLevel.Information,
                formatString: @"Scheme {scheme} - Handler {handler} - Event {event}
Description:
This event is raised before SignIn the current ticket and redirect to the original requested uri in the site.
Information:
ReturnUri: {returntUri}
Useful for:
Change the return uri or handle or skip the signin.");
            _remoteFailure = LoggerMessage.Define<string, string>(
                eventId: new EventId(4, "RemoteFailure"),
                logLevel: LogLevel.Information,
                formatString: @"Scheme {scheme} - Handler {handler} - Event {event}
Description:
This event is raised before SignIn the current ticket and redirect to the original requested uri in the site.
Information:
ReturnUri: {returntUri}
Useful for:
Change the return uri or handle or skip the signin.");
        }

        public static void RemoteAuthenticationError(this ILogger logger, string scheme, string redirectUri)
        {
            _redirectToAuthorizationEndpoint(
                logger,
                scheme,
                nameof(RemoteAuthenticationError),
                redirectUri,
                null);
        }

        public static void CreatingTicket(this ILogger logger, string scheme, string accessToken)
        {
            _creatingTicket(
                logger,
                scheme,
                nameof(CreatingTicket),
                accessToken,
                null);
        }

        public static void TicketReceived(this ILogger logger, string scheme, string returntUri)
        {
            _ticketReceived(
                logger,
                scheme,
                nameof(TicketReceived),
                returntUri,
                null);
        }

        public static void RemoteFailure(this ILogger logger, string scheme, Exception failure)
        {
            _remoteFailure(
                logger,
                scheme,
                nameof(RemoteFailure),
                failure);
        }
    }
}
