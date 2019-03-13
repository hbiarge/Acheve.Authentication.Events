using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication.WsFederation
{
    public class LogWsFederationEvents : WsFederationEvents
    {
        private readonly ILogger<LogWsFederationEvents> _logger;

        public LogWsFederationEvents(ILogger<LogWsFederationEvents> logger)
        {
            _logger = logger;
        }

        public override Task RedirectToIdentityProvider(RedirectContext context)
        {
            _logger.LogInformation("Scheme {scheme}: RedirectToIdentityProvider called...", context.Scheme.Name);
            return base.RedirectToIdentityProvider(context);
        }

        public override Task MessageReceived(MessageReceivedContext context)
        {
            _logger.LogInformation("Scheme {scheme}: MessageReceived called...", context.Scheme.Name);
            return base.MessageReceived(context);
        }

        public override Task SecurityTokenReceived(SecurityTokenReceivedContext context)
        {
            _logger.LogInformation("Scheme {scheme}: SecurityTokenReceived called...", context.Scheme.Name);
            return base.SecurityTokenReceived(context);
        }

        public override Task SecurityTokenValidated(SecurityTokenValidatedContext context)
        {
            _logger.LogInformation("Scheme {scheme}: SecurityTokenValidated called...", context.Scheme.Name);
            return base.SecurityTokenValidated(context);
        }

        public override Task TicketReceived(TicketReceivedContext context)
        {
            _logger.TicketReceived(
                scheme: context.Scheme.Name,
                returntUri: context.ReturnUri);

            return base.TicketReceived(context);
        }

        public override Task RemoteSignOut(RemoteSignOutContext context)
        {
            _logger.LogInformation("Scheme {scheme}: RemoteSignOut called...", context.Scheme.Name);
            return base.RemoteSignOut(context);
        }

        public override Task RemoteFailure(RemoteFailureContext context)
        {
            _logger.RemoteFailure(
                scheme: context.Scheme.Name,
                failure: context.Failure);

            return base.RemoteFailure(context);
        }

        public override Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            _logger.LogInformation("Scheme {scheme}: AuthenticationFailed called...", context.Scheme.Name);
            return base.AuthenticationFailed(context);
        }
    }
}
