using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication.Facebook
{
    public class LogFacebookEvents : OAuthEvents
    {
        private readonly ILogger<LogFacebookEvents> _logger;

        public LogFacebookEvents(ILogger<LogFacebookEvents> logger)
        {
            _logger = logger;
        }

        public override Task RedirectToAuthorizationEndpoint(RedirectContext<OAuthOptions> context)
        {
            _logger.LogInformation("Scheme {scheme}: RedirectToAuthorizationEndpoint called...", context.Scheme.Name);
            return base.RedirectToAuthorizationEndpoint(context);
        }

        public override Task TicketReceived(TicketReceivedContext context)
        {
            _logger.LogInformation("Scheme {scheme}: TicketReceived called...", context.Scheme.Name);
            return base.TicketReceived(context);
        }

        public override Task CreatingTicket(OAuthCreatingTicketContext context)
        {
            _logger.LogInformation("Scheme {scheme}: CreatingTicket called...", context.Scheme.Name);
            return base.CreatingTicket(context);
        }

        public override Task RemoteFailure(RemoteFailureContext context)
        {
            _logger.LogInformation("Scheme {scheme}: RemoteFailure called...", context.Scheme.Name);
            return base.RemoteFailure(context);
        }
    }
}
