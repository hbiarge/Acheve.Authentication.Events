using System;
using Acheve.Authentication.Events.Remote;
using Acheve.Authentication.Events.Remote.WsFederation;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication
{
    public class EventsTypeWsFederationPostConfigureOptions : IPostConfigureOptions<WsFederationOptions>
    {
        public void PostConfigure(string name, WsFederationOptions options)
        {
            options.EventsType = typeof(LogWsFederationEvents);
        }
    }

    public class LogWsFederationEvents : WsFederationEvents
    {
        private readonly IOptionsMonitor<WsFederationOptions> _options;
        private readonly ILogger<LogWsFederationEvents> _logger;

        public LogWsFederationEvents(IOptionsMonitor<WsFederationOptions> options, ILogger<LogWsFederationEvents> logger)
        {
            _options = options;
            _logger = logger;
        }

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

        private async Task SafeCallOriginalEvent(WsFederationEvents events, Func<WsFederationEvents, Task> action)
        {
            if (events != null)
            {
                await action(events);
            }
        }
    }
}
