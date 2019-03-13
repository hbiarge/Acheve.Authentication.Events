using Acheve.Authentication.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication.Twitter
{
    public class LogTwitterEvents : TwitterEvents
    {
        private readonly ILogger<LogTwitterEvents> _logger;

        public LogTwitterEvents(ILogger<LogTwitterEvents> logger)
        {
            _logger = logger;
        }

        public override Task RedirectToAuthorizationEndpoint(RedirectContext<TwitterOptions> context)
        {
            _logger.LogInformation(
                @"Scheme {scheme} - Event {event}:
Description:
This is a {handlerType}. You are going to be redirected to the Identity provider.
Information:
RedirectUri: {redirectUri}
Useful for:
This is your last chance to customize the redirect url before being redirected.",
                context.Scheme.Name,
                nameof(RedirectToAuthorizationEndpoint),
                Constants.RemoteAuthenticatuionHandler,
                context.RedirectUri);

            return base.RedirectToAuthorizationEndpoint(context);
        }

        public override Task CreatingTicket(TwitterCreatingTicketContext context)
        {
            _logger.LogInformation(
                @"Scheme {scheme}  - Event {event}:
Description:
The identity provider has authenticated the user, redirected to the application and the handler has created the ClaimsPrincipal.
Information:
You can review the principal created and the information the identity provider has sent.
UserId: {userId}
AccessToken: {accessToken}
AccessTokenSecret: {accessTokenSecret}
Useful for:
You can add some custom information to the ClaimsPrincipal or implement additional verification code to decide if the user is authenticated or not.",
                context.Scheme.Name,
                nameof(CreatingTicket),
                context.UserId,
                context.AccessToken,
                context.AccessTokenSecret);

            return base.CreatingTicket(context);
        }

        public override Task TicketReceived(TicketReceivedContext context)
        {
            _logger.LogInformation(
                @"Scheme {scheme}  - Event {event}:
Description:
This event is raised before {signinIn} the current ticket and redirect to the original requested uri in the site.
Information:
ReturnUri: {returntUri}
Useful for:
Change the return uri or handle or skip the signin.
", 
                context.Scheme.Name,
                nameof(TicketReceived),
                nameof(AuthenticationHttpContextExtensions.SignInAsync),
                context.ReturnUri);

            return base.TicketReceived(context);
        }

        public override Task RemoteFailure(RemoteFailureContext context)
        {
            _logger.LogInformation("Scheme {scheme}: RemoteFailure called...", context.Scheme.Name);
            return base.RemoteFailure(context);
        }
    }
}
