using System;
using System.Runtime.InteropServices.ComTypes;
using Acheve.Authentication.Events.Remote;
using Acheve.Authentication.Events.Remote.WsFederation;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication;

public class EventsTypeWsFederationPostConfigureOptions : IPostConfigureOptions<WsFederationOptions>
{
    public void PostConfigure(string? name, WsFederationOptions options)
    {
        if (options.EventsType is null)
        {
            options.EventsType = typeof(LogWsFederationEvents);
        }
        else
        {
            options.EventsType = typeof(LogWsFederationEvents<>).MakeGenericType(options.EventsType);
        }
    }
}

public class LogWsFederationEvents(
    IOptionsMonitor<WsFederationOptions> options,
    ILogger<LogWsFederationEvents> logger) : WsFederationEvents
{
    private readonly IOptionsMonitor<WsFederationOptions> _options = options;
    private readonly ILogger<LogWsFederationEvents> _logger = logger;

    public override async Task RedirectToIdentityProvider(RedirectContext context)
    {
        _logger.RedirectToIdentityProvider(
            scheme: context.Scheme.Name,
            protocolMessage: context.ProtocolMessage);

        await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.RedirectToIdentityProvider(context));

        await base.RedirectToIdentityProvider(context);
    }

    public override async Task MessageReceived(MessageReceivedContext context)
    {
        _logger.MessageReceived(
            scheme: context.Scheme.Name,
            protocolMessage: context.ProtocolMessage);

        await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.MessageReceived(context));

        await base.MessageReceived(context);
    }

    public override async Task SecurityTokenReceived(SecurityTokenReceivedContext context)
    {
        _logger.SecurityTokenReceived(
            scheme: context.Scheme.Name,
            protocolMessage: context.ProtocolMessage,
            principal: context.Principal);

        await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.SecurityTokenReceived(context));

        await base.SecurityTokenReceived(context);
    }

    public override async Task SecurityTokenValidated(SecurityTokenValidatedContext context)
    {
        _logger.SecurityTokenValidated(
            scheme: context.Scheme.Name,
            protocolMessage: context.ProtocolMessage,
            principal: context.Principal);

        await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.SecurityTokenValidated(context));

        await base.SecurityTokenValidated(context);
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

    public override async Task RemoteSignOut(RemoteSignOutContext context)
    {
        _logger.RemoteSignOut(
            scheme: context.Scheme.Name,
            protocolMessage: context.ProtocolMessage);

        await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.RemoteSignOut(context));

        await base.RemoteSignOut(context);
    }

    public override async Task RemoteFailure(RemoteFailureContext context)
    {
        _logger.RemoteFailure(
            scheme: context.Scheme.Name,
            failure: context.Failure);

        await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.RemoteFailure(context));

        await base.RemoteFailure(context);
    }

    public override async Task AuthenticationFailed(AuthenticationFailedContext context)
    {
        _logger.AuthenticationFailed(
            scheme: context.Scheme.Name,
            exception: context.Exception);

        await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.AuthenticationFailed(context));

        await base.AuthenticationFailed(context);
    }

    private static async Task SafeCallOriginalEvent(WsFederationEvents? events, Func<WsFederationEvents, Task> action)
    {
        if (events is not null)
        {
            await action(events);
        }
    }
}

public class LogWsFederationEvents<TOther>(
    IOptionsMonitor<WsFederationOptions> options,
    TOther originalEvents,
    ILogger<LogWsFederationEvents> logger)
    : LogWsFederationEvents(options, logger)
    where TOther : WsFederationEvents
{
    private readonly TOther _originalEvents = originalEvents;

    public override async Task RedirectToIdentityProvider(RedirectContext context)
    {
        await base.RedirectToIdentityProvider(context);
        await _originalEvents.RedirectToIdentityProvider(context);
    }

    public override async Task MessageReceived(MessageReceivedContext context)
    {
        await base.MessageReceived(context);
        await _originalEvents.MessageReceived(context);
    }

    public override async Task SecurityTokenReceived(SecurityTokenReceivedContext context)
    {
        await base.SecurityTokenReceived(context);
        await _originalEvents.SecurityTokenReceived(context);
    }

    public override async Task SecurityTokenValidated(SecurityTokenValidatedContext context)
    {
        await base.SecurityTokenValidated(context);
        await _originalEvents.SecurityTokenValidated(context);
    }

    public override async Task TicketReceived(TicketReceivedContext context)
    {
        await base.TicketReceived(context);
        await _originalEvents.TicketReceived(context);
    }

    public override async Task RemoteSignOut(RemoteSignOutContext context)
    {
        await base.RemoteSignOut(context);
        await _originalEvents.RemoteSignOut(context);
    }

    public override async Task RemoteFailure(RemoteFailureContext context)
    {
        await base.RemoteFailure(context);
        await _originalEvents.RemoteFailure(context);
    }

    public override async Task AuthenticationFailed(AuthenticationFailedContext context)
    {
        await base.AuthenticationFailed(context);
        await _originalEvents.AuthenticationFailed(context);
    }
}
