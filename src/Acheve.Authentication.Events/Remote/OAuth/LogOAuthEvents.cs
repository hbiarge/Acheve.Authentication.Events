using System;
using Acheve.Authentication.Events.Remote;
using Acheve.Authentication.Events.Remote.OAuth;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication;

public class EventsTypeOAuthPostConfigureOptions : IPostConfigureOptions<OAuthOptions>
{
    public void PostConfigure(string? name, OAuthOptions options)
    {
        if (options.EventsType is null)
        {
            options.EventsType = typeof(LogOAuthEvents);
        }
        else
        {
            options.EventsType = typeof(LogOAuthEvents<>).MakeGenericType(options.EventsType);
        }
    }
}

public class LogOAuthEvents(
    IOptionsMonitor<OAuthOptions> options,
    ILogger<LogOAuthEvents> logger) : OAuthEvents
{
    private readonly IOptionsMonitor<OAuthOptions> _options = options;
    private readonly ILogger<LogOAuthEvents> _logger = logger;

    public override async Task RedirectToAuthorizationEndpoint(RedirectContext<OAuthOptions> context)
    {
        _logger.RedirectToAuthorizationEndpoint(
            scheme: context.Scheme.Name,
            redirectUri: context.RedirectUri);

        await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.RedirectToAuthorizationEndpoint(context));

        await base.RedirectToAuthorizationEndpoint(context);
    }

    public override async Task CreatingTicket(OAuthCreatingTicketContext context)
    {
        _logger.CreatingTicket(
             scheme: context.Scheme.Name,
             accessToken: context.AccessToken,
             principal: context.Principal);

        await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.CreatingTicket(context));

        await base.CreatingTicket(context);
    }

    public override async Task TicketReceived(TicketReceivedContext context)
    {
        _logger.TicketReceived(
             scheme: context.Scheme.Name,
             principal: context.Principal,
             returntUri: context.ReturnUri);

        await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.TicketReceived(context));

        await base.TicketReceived(context);
    }

    public override async Task RemoteFailure(RemoteFailureContext context)
    {
        _logger.RemoteFailure(
            scheme: context.Scheme.Name,
            failure: context.Failure);

        await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.RemoteFailure(context));

        await base.RemoteFailure(context);
    }

    private static async Task SafeCallOriginalEvent(OAuthEvents? events, Func<OAuthEvents, Task> action)
    {
        if (events is not null)
        {
            await action(events);
        }
    }
}

public class LogOAuthEvents<TOther>(
    IOptionsMonitor<OAuthOptions> options,
    TOther originalEvents,
    ILogger<LogOAuthEvents> logger)
    : LogOAuthEvents(options, logger)
    where TOther : OAuthEvents
{
    private readonly TOther _originalEvents = originalEvents;

    public override async Task RedirectToAuthorizationEndpoint(RedirectContext<OAuthOptions> context)
    {
        await base.RedirectToAuthorizationEndpoint(context);
        await _originalEvents.RedirectToAuthorizationEndpoint(context);
    }

    public override async Task CreatingTicket(OAuthCreatingTicketContext context)
    {
        await base.CreatingTicket(context);
        await _originalEvents.CreatingTicket(context);
    }

    public override async Task TicketReceived(TicketReceivedContext context)
    {
        await base.TicketReceived(context);
        await _originalEvents.TicketReceived(context);
    }

    public override async Task RemoteFailure(RemoteFailureContext context)
    {
        await base.RemoteFailure(context);
        await _originalEvents.RemoteFailure(context);
    }
}
