using System;
using Acheve.Authentication.Events.Remote;
using Acheve.Authentication.Events.Remote.OAuth;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication
{
    public class EventsTypeOAuthPostConfigureOptions : IPostConfigureOptions<OAuthOptions>
    {
        public void PostConfigure(string name, OAuthOptions options)
        {
            options.EventsType = typeof(LogOAuthEvents);
        }
    }

    public class LogOAuthEvents : OAuthEvents
    {
        private readonly IOptionsMonitor<OAuthOptions> _options;
        private readonly ILogger<LogOAuthEvents> _logger;

        public LogOAuthEvents(IOptionsMonitor<OAuthOptions> options, ILogger<LogOAuthEvents> logger)
        {
            _options = options;
            _logger = logger;
        }

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

        private async Task SafeCallOriginalEvent(OAuthEvents events, Func<OAuthEvents, Task> action)
        {
            if (events != null)
            {
                await action(events);
            }
        }
    }
}
