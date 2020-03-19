using System;
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
            if (options.EventsType is null)
            {
                options.EventsType = typeof(LogOpenIdConnectEvents);
            }
            else
            {
                options.EventsType = typeof(LogOpenIdConnectEvents<>).MakeGenericType(options.EventsType);
            }
        }
    }

    public class LogOpenIdConnectEvents : OpenIdConnectEvents
    {
        private readonly IOptionsMonitor<OpenIdConnectOptions> _options;
        private readonly ILogger<LogOpenIdConnectEvents> _logger;

        public LogOpenIdConnectEvents(IOptionsMonitor<OpenIdConnectOptions> options, ILogger<LogOpenIdConnectEvents> logger)
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

        public override async Task TokenValidated(TokenValidatedContext context)
        {
            _logger.TokenValidated(
                scheme: context.Scheme.Name,
                protocolMessage: context.ProtocolMessage,
                principal: context.Principal);

            await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.TokenValidated(context));

            await base.TokenValidated(context);
        }

        public override async Task AuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
        {
            _logger.AuthorizationCodeReceived(
                scheme: context.Scheme.Name,
                protocolMessage: context.ProtocolMessage,
                principal: context.Principal);

            await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.AuthorizationCodeReceived(context));

            await base.AuthorizationCodeReceived(context);
        }

        public override async Task TokenResponseReceived(TokenResponseReceivedContext context)
        {
            _logger.TokenResponseReceived(
                scheme: context.Scheme.Name,
                protocolMessage: context.ProtocolMessage,
                tokenEndpointResponse: context.TokenEndpointResponse,
                principal: context.Principal);

            await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.TokenResponseReceived(context));

            await base.TokenResponseReceived(context);
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

        public override async Task UserInformationReceived(UserInformationReceivedContext context)
        {
            _logger.UserInformationReceived(
                scheme: context.Scheme.Name,
                protocolMessage: context.ProtocolMessage,
                principal: context.Principal,
                user: context.User);

            await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.UserInformationReceived(context));

            await base.UserInformationReceived(context);
        }

        public override async Task RedirectToIdentityProviderForSignOut(RedirectContext context)
        {
            _logger.RedirectToIdentityProviderForSignOut(
                scheme: context.Scheme.Name,
                protocolMessage: context.ProtocolMessage,
                options: context.Options,
                properties: context.Properties);

            await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.RedirectToIdentityProviderForSignOut(context));

            await base.RedirectToIdentityProviderForSignOut(context);
        }

        public override async Task SignedOutCallbackRedirect(RemoteSignOutContext context)
        {
            _logger.SignedOutCallbackRedirect(
                scheme: context.Scheme.Name,
                properties: context.Properties);

            await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.SignedOutCallbackRedirect(context));

            await base.SignedOutCallbackRedirect(context);
        }

        public override async Task RemoteSignOut(RemoteSignOutContext context)
        {
            _logger.RemoteSignOut(
                scheme: context.Scheme.Name,
                protocolMessage: context.ProtocolMessage);

            await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.RemoteSignOut(context));

            await base.RemoteSignOut(context);
        }

        public override async Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            _logger.AuthenticationFailed(
                scheme: context.Scheme.Name,
                exception: context.Exception);

            await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.AuthenticationFailed(context));

            await base.AuthenticationFailed(context);
        }

        public override async Task RemoteFailure(RemoteFailureContext context)
        {
            _logger.RemoteFailure(
                scheme: context.Scheme.Name,
                failure: context.Failure);

            await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.RemoteFailure(context));

            await base.RemoteFailure(context);
        }

        private static async Task SafeCallOriginalEvent(OpenIdConnectEvents events, Func<OpenIdConnectEvents, Task> action)
        {
            if (events != null)
            {
                await action(events);
            }
        }
    }

    public class LogOpenIdConnectEvents<TOther> : LogOpenIdConnectEvents where TOther : OpenIdConnectEvents
    {
        private readonly TOther _originalEvents;

        public LogOpenIdConnectEvents(IOptionsMonitor<OpenIdConnectOptions> options, TOther originalEvents, ILogger<LogOpenIdConnectEvents> logger)
            : base(options, logger)
        {
            _originalEvents = originalEvents;
        }

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

        public override async Task TokenValidated(TokenValidatedContext context)
        {
            await base.TokenValidated(context);
            await _originalEvents.TokenValidated(context);
        }

        public override async Task AuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
        {
            await base.AuthorizationCodeReceived(context);
            await _originalEvents.AuthorizationCodeReceived(context);
        }

        public override async Task TokenResponseReceived(TokenResponseReceivedContext context)
        {
            await base.TokenResponseReceived(context);
            await _originalEvents.TokenResponseReceived(context);
        }

        public override async Task TicketReceived(TicketReceivedContext context)
        {
            await base.TicketReceived(context);
            await _originalEvents.TicketReceived(context);
        }

        public override async Task UserInformationReceived(UserInformationReceivedContext context)
        {
            await base.UserInformationReceived(context);
            await _originalEvents.UserInformationReceived(context);
        }

        public override async Task RedirectToIdentityProviderForSignOut(RedirectContext context)
        {
            await base.RedirectToIdentityProviderForSignOut(context);
            await _originalEvents.RedirectToIdentityProviderForSignOut(context);
        }

        public override async Task SignedOutCallbackRedirect(RemoteSignOutContext context)
        {
            await base.SignedOutCallbackRedirect(context);
            await _originalEvents.SignedOutCallbackRedirect(context);
        }

        public override async Task RemoteSignOut(RemoteSignOutContext context)
        {
            await base.RemoteSignOut(context);
            await _originalEvents.RemoteSignOut(context);
        }

        public override async Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            await base.AuthenticationFailed(context);
            await _originalEvents.AuthenticationFailed(context);
        }

        public override async Task RemoteFailure(RemoteFailureContext context)
        {
            await base.RemoteFailure(context);
            await _originalEvents.RemoteFailure(context);
        }
    }
}
