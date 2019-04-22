using Acheve.Authentication.Events.Remote;
using Acheve.Authentication.Events.Remote.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication
{
    public class EventsTypeOpenIdConnectPostConfigureOptions : IPostConfigureOptions<OpenIdConnectOptions>
    {
        public void PostConfigure(string name, OpenIdConnectOptions options)
        {
            options.EventsType = typeof(LogOpenIdConnectEvents);
        }
    }

    public class LogOpenIdConnectEvents : OpenIdConnectEvents
    {
        private readonly ILogger<LogOpenIdConnectEvents> _logger;

        public LogOpenIdConnectEvents(ILogger<LogOpenIdConnectEvents> logger)
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

        public override Task TokenValidated(TokenValidatedContext context)
        {
            _logger.TokenValidated(
                scheme: context.Scheme.Name,
                protocolMessage: context.ProtocolMessage,
                principal: context.Principal);

            return base.TokenValidated(context);
        }

        public override Task AuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
        {
            _logger.AuthorizationCodeReceived(
                scheme: context.Scheme.Name,
                protocolMessage: context.ProtocolMessage,
                principal: context.Principal);

            return base.AuthorizationCodeReceived(context);
        }

        public override Task TokenResponseReceived(TokenResponseReceivedContext context)
        {
            _logger.TokenResponseReceived(
                scheme: context.Scheme.Name,
                protocolMessage: context.ProtocolMessage,
                tokenEndpointResponse: context.TokenEndpointResponse,
                principal: context.Principal);

            return base.TokenResponseReceived(context);
        }

        public override Task TicketReceived(TicketReceivedContext context)
        {
            _logger.TicketReceived(
                scheme: context.Scheme.Name,
                principal: context.Principal,
                returntUri: context.ReturnUri);

            return base.TicketReceived(context);
        }

        public override Task UserInformationReceived(UserInformationReceivedContext context)
        {
            _logger.UserInformationReceived(
                scheme: context.Scheme.Name,
                protocolMessage: context.ProtocolMessage,
                principal: context.Principal,
                user: context.User);

            return base.UserInformationReceived(context);
        }

        public override Task RedirectToIdentityProviderForSignOut(RedirectContext context)
        {
            _logger.RedirectToIdentityProviderForSignOut(
                scheme: context.Scheme.Name,
                protocolMessage: context.ProtocolMessage,
                options: context.Options,
                properties: context.Properties);

            return base.RedirectToIdentityProviderForSignOut(context);
        }

        public override Task SignedOutCallbackRedirect(RemoteSignOutContext context)
        {
            _logger.SignedOutCallbackRedirect(
                scheme: context.Scheme.Name,
                properties: context.Properties);

            return base.SignedOutCallbackRedirect(context);
        }

        public override Task RemoteSignOut(RemoteSignOutContext context)
        {
            _logger.RemoteSignOut(
                scheme: context.Scheme.Name,
                protocolMessage: context.ProtocolMessage);

            return base.RemoteSignOut(context);
        }

        public override Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            _logger.AuthenticationFailed(
                scheme: context.Scheme.Name,
                exception: context.Exception);

            return base.AuthenticationFailed(context);
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
