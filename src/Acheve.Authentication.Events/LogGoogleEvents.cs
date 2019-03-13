﻿using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication.Google
{
    public class LogGoogleEvents : OAuthEvents
    {
        private readonly ILogger<LogGoogleEvents> _logger;

        public LogGoogleEvents(ILogger<LogGoogleEvents> logger)
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
