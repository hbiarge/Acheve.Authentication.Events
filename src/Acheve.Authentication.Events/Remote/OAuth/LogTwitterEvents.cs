using System;
using Acheve.Authentication.Events.Remote;
using Acheve.Authentication.Events.Remote.OAuth;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication;

public class EventsTypeTwitterPostConfigureOptions : IPostConfigureOptions<TwitterOptions>
{
    public void PostConfigure(string? name, TwitterOptions options)
    {
        if (options.EventsType is null)
        {
            options.EventsType = typeof(LogTwitterEvents);
        }
        else
        {
            options.EventsType = typeof(LogTwitterEvents<>).MakeGenericType(options.EventsType);
        }
    }
}

public class LogTwitterEvents(
    IOptionsMonitor<TwitterOptions> options,
    ILogger<LogTwitterEvents> logger) : TwitterEvents
{
    private readonly IOptionsMonitor<TwitterOptions> _options = options;
    private readonly ILogger<LogTwitterEvents> _logger = logger;

    public override async Task RedirectToAuthorizationEndpoint(RedirectContext<TwitterOptions> context)
    {
        _logger.RedirectToAuthorizationEndpoint(
            scheme: context.Scheme.Name,
            redirectUri: context.RedirectUri);

        await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.RedirectToAuthorizationEndpoint(context));

        await base.RedirectToAuthorizationEndpoint(context);
    }

    public override async Task CreatingTicket(TwitterCreatingTicketContext context)
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

    private static async Task SafeCallOriginalEvent(TwitterEvents? events, Func<TwitterEvents, Task> action)
    {
        if (events is not null)
        {
            await action(events);
        }
    }
}

public class LogTwitterEvents<TOther>(
    IOptionsMonitor<TwitterOptions> options,
    TOther originalEvents,
    ILogger<LogTwitterEvents> logger)
    : LogTwitterEvents(options, logger)
    where TOther : TwitterEvents
{
    private readonly TOther _originalEvents = originalEvents;

    public override async Task RedirectToAuthorizationEndpoint(RedirectContext<TwitterOptions> context)
    {
        await base.RedirectToAuthorizationEndpoint(context);
        await _originalEvents.RedirectToAuthorizationEndpoint(context);
    }

    public override async Task CreatingTicket(TwitterCreatingTicketContext context)
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
