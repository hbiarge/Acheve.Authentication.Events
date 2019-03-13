using Acheve.Authentication.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication.OAuth
{
    public class LogOAuthEvents : OAuthEvents
    {
        private readonly ILogger<LogOAuthEvents> _logger;

        public LogOAuthEvents(ILogger<LogOAuthEvents> logger)
        {
            _logger = logger;
        }

        public override Task RedirectToAuthorizationEndpoint(RedirectContext<OAuthOptions> context)
        {
            _logger.LogInformation(
                @"Scheme {scheme} - Event {event}: This is a {handlerType}. You are going to be redirected to the Identity provider.
This is your last chance to customize the redirect url before being redirected.",
                context.Scheme.Name,
                nameof(RedirectToAuthorizationEndpoint),
                Constants.RemoteAuthenticatuionHandler);
            return base.RedirectToAuthorizationEndpoint(context);
        }

        public override Task CreatingTicket(OAuthCreatingTicketContext context)
        {
            _logger.LogInformation("Scheme {scheme}: CreatingTicket called...", context.Scheme.Name);
            return base.CreatingTicket(context);
        }

        public override Task TicketReceived(TicketReceivedContext context)
        {
            _logger.LogInformation("Scheme {scheme}: TicketReceived called...", context.Scheme.Name);
            return base.TicketReceived(context);
        }

        public override Task RemoteFailure(RemoteFailureContext context)
        {
            _logger.LogInformation("Scheme {scheme}: RemoteFailure called...", context.Scheme.Name);
            return base.RemoteFailure(context);
        }
    }
}
