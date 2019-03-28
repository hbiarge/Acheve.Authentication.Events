using Acheve.Authentication.Events.Remote;
using Acheve.Authentication.Events.Remote.OAuth;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication.MicrosoftAccount
{
    public class EventsTypeMicrosoftAccountPostConfigureOptions : IPostConfigureOptions<MicrosoftAccountOptions>
    {
        public void PostConfigure(string name, MicrosoftAccountOptions options)
        {
            options.EventsType = typeof(LogMicrosoftAccountEvents);
        }
    }

    public class LogMicrosoftAccountEvents : OAuthEvents
    {
        private readonly ILogger<LogMicrosoftAccountEvents> _logger;

        public LogMicrosoftAccountEvents(ILogger<LogMicrosoftAccountEvents> logger)
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
                 accessToken: context.AccessToken,
                 principal: context.Principal);

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
