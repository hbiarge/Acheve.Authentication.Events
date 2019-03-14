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
            _logger.RedirectToAuthorizationEndpoint(
                scheme: context.Scheme.Name,
                redirectUri: context.RedirectUri);

            return base.RedirectToAuthorizationEndpoint(context);
        }

        public override Task CreatingTicket(OAuthCreatingTicketContext context)
        {
            _logger.CreatingTicket(
                 scheme: context.Scheme.Name,
                 accessToken: context.AccessToken);

            return base.CreatingTicket(context);
        }

        public override Task TicketReceived(TicketReceivedContext context)
        {
            _logger.TicketReceived(
                 scheme: context.Scheme.Name,
                 principal: context.Principal,
                 returntUri: context.ReturnUri);

            return base.TicketReceived(context);
        }

        public override Task RemoteFailure(RemoteFailureContext context)
        {
            _logger.RemoteFailure(
                scheme: context.Scheme.Name,
                failure: context.Failure);

            return base.RemoteFailure(context);
        }
    }
}
