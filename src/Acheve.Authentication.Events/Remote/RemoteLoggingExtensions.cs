using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace Acheve.Authentication.Events.Remote;

/// <summary>
///    OAuth (OAuth, Twitter, Google, Facebook and MicrosoftAccount)
/// </summary>
internal static class RemoteLoggingExtensions
{
    private static readonly Action<ILogger, string, string, string, string, Exception> _ticketReceived;
    private static readonly Action<ILogger, string, string, string, Exception> _remoteFailure;

    static RemoteLoggingExtensions()
    {
        _ticketReceived = LoggerMessage.Define<string, string, string, string>(
            eventId: new EventId(3, "TicketReceived"),
            logLevel: LogLevel.Information,
            formatString: @"Scheme {scheme} - Event {event}
Description:
This event is raised before SignIn the current ticket and redirect to the original requested uri in the site.
Relevant information:
Principal: {principal}
ReturnUri: {returntUri}
Useful for:
Change the return uri or handle or skip the signin.");

        _remoteFailure = LoggerMessage.Define<string, string, string>(
            eventId: new EventId(4, "RemoteFailure"),
            logLevel: LogLevel.Information,
            formatString: @"Scheme {scheme} - Event {event}
Description:
This event is raised before SignIn the current ticket and redirect to the original requested uri in the site.
Relevant information:
failure: {failure}
Useful for:
Change the return uri or handle or skip the signin.");
    }

    public static void TicketReceived(this ILogger logger, string scheme, ClaimsPrincipal? principal, string? returntUri)
    {
        _ticketReceived(
            logger,
            scheme,
            nameof(TicketReceived),
            principal?.ToFormattedLog() ?? "N/A",
            returntUri ?? "N/A",
            null);
    }

    public static void RemoteFailure(this ILogger logger, string scheme, Exception? failure)
    {
        _remoteFailure(
            logger,
            scheme,
            nameof(RemoteFailure),
            failure?.ToString() ?? "N/A",
            null);
    }
}
