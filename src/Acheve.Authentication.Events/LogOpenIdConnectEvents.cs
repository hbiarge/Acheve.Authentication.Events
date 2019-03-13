using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication.OpenIdConnect
{
    public class LogOpenIdConnectEvents : OpenIdConnectEvents
    {
        private readonly ILogger<LogOpenIdConnectEvents> _logger;

        public LogOpenIdConnectEvents(ILogger<LogOpenIdConnectEvents> logger)
        {
            _logger = logger;
        }

        public override Task RedirectToIdentityProvider(RedirectContext context)
        {
            _logger.LogInformation("Scheme {scheme}: RedirectToIdentityProvider called. You are going to be redirected to {authEndpoint}.", context.Scheme.Name, context.ProtocolMessage.AuthorizationEndpoint);
            return base.RedirectToIdentityProvider(context);
        }

        public override Task MessageReceived(MessageReceivedContext context)
        {
            _logger.LogInformation("Scheme {scheme}: MessageReceived called. Ok, the identity provider has sent", context.Scheme.Name);
            return base.MessageReceived(context);
        }

        public override Task AuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
        {
            _logger.LogInformation("Scheme {scheme}: AuthorizationCodeReceived called...", context.Scheme.Name);
            return base.AuthorizationCodeReceived(context);
        }

        public override Task TokenResponseReceived(TokenResponseReceivedContext context)
        {
            _logger.LogInformation("Scheme {scheme}: TokenResponseReceived called...", context.Scheme.Name);
            return base.TokenResponseReceived(context);
        }

        public override Task TicketReceived(TicketReceivedContext context)
        {
            _logger.LogInformation("Scheme {scheme}: TicketReceived called...", context.Scheme.Name);
            return base.TicketReceived(context);
        }

        public override Task UserInformationReceived(UserInformationReceivedContext context)
        {
            _logger.LogInformation("Scheme {scheme}: UserInformationReceived called...", context.Scheme.Name);
            return base.UserInformationReceived(context);
        }

        public override Task TokenValidated(TokenValidatedContext context)
        {
            _logger.LogInformation("Scheme {scheme}: TokenValidated called...", context.Scheme.Name);
            return base.TokenValidated(context);
        }

        public override Task RedirectToIdentityProviderForSignOut(RedirectContext context)
        {
            _logger.LogInformation("Scheme {scheme}: RedirectToIdentityProviderForSignOut called...", context.Scheme.Name);
            return base.RedirectToIdentityProviderForSignOut(context);
        }

        public override Task RemoteSignOut(RemoteSignOutContext context)
        {
            _logger.LogInformation("Scheme {scheme}: RemoteSignOut called...", context.Scheme.Name);
            return base.RemoteSignOut(context);
        }

        public override Task SignedOutCallbackRedirect(RemoteSignOutContext context)
        {
            _logger.LogInformation("Scheme {scheme}: SignedOutCallbackRedirect called...", context.Scheme.Name);
            return base.SignedOutCallbackRedirect(context);
        }

        public override Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            _logger.LogInformation("Scheme {scheme}: AuthenticationFailed called...", context.Scheme.Name);
            return base.AuthenticationFailed(context);
        }

        public override Task RemoteFailure(RemoteFailureContext context)
        {
            _logger.LogInformation("Scheme {scheme}: RemoteFailure called...", context.Scheme.Name);
            return base.RemoteFailure(context);
        }
    }
}
