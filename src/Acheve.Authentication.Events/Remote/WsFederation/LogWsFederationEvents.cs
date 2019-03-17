using Acheve.Authentication.Events.Remote;
using Acheve.Authentication.Events.Remote.WsFederation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication.WsFederation
{
    public class WsFederationOptionsConfiguration : IPostConfigureOptions<WsFederationOptions>
    {
        public void PostConfigure(string name, WsFederationOptions options)
        {
            options.EventsType = typeof(LogWsFederationEvents);
        }
    }

    public class LogWsFederationEvents : WsFederationEvents
    {
        private readonly ILogger<LogWsFederationEvents> _logger;

        public LogWsFederationEvents(ILogger<LogWsFederationEvents> logger)
        {
            _logger = logger;
        }

        public override Task RedirectToIdentityProvider(RedirectContext context)
        {
            _logger.RedirectToIdentityProvider(
                scheme: context.Scheme.Name,
                protocolMessage: context.ProtocolMessage);

            return base.RedirectToIdentityProvider(context);
        }

        public override Task MessageReceived(MessageReceivedContext context)
        {
            _logger.MessageReceived(
                scheme: context.Scheme.Name,
                protocolMessage: context.ProtocolMessage);

            return base.MessageReceived(context);
        }

        public override Task SecurityTokenReceived(SecurityTokenReceivedContext context)
        {
            _logger.SecurityTokenReceived(
                scheme: context.Scheme.Name,
                protocolMessage: context.ProtocolMessage,
                principal: context.Principal);

            return base.SecurityTokenReceived(context);
        }

        public override Task SecurityTokenValidated(SecurityTokenValidatedContext context)
        {
            _logger.SecurityTokenValidated(
                scheme: context.Scheme.Name,
                protocolMessage: context.ProtocolMessage,
                principal: context.Principal);

            return base.SecurityTokenValidated(context);
        }

        public override Task TicketReceived(TicketReceivedContext context)
        {
            _logger.TicketReceived(
                scheme: context.Scheme.Name,
                principal: context.Principal,
                returntUri: context.ReturnUri);

            return base.TicketReceived(context);
        }

        public override Task RemoteSignOut(RemoteSignOutContext context)
        {
            _logger.RemoteSignOut(
                scheme: context.Scheme.Name,
                protocolMessage: context.ProtocolMessage);

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
            _logger.AuthenticationFailed(
                scheme: context.Scheme.Name,
                exception: context.Exception);

            return base.AuthenticationFailed(context);
        }
    }
}
