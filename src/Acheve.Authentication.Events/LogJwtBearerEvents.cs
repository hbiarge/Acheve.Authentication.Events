using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication.JwtBearer
{
    public class LogJwtBearerEvents : JwtBearerEvents
    {
        private readonly ILogger<LogJwtBearerEvents> _logger;

        public LogJwtBearerEvents(ILogger<LogJwtBearerEvents> logger)
        {
            _logger = logger;
        }

        public override Task Challenge(JwtBearerChallengeContext context)
        {
            _logger.LogInformation("Scheme {scheme}: Challenge called...", context.Scheme.Name);
            return base.Challenge(context);
        }

        public override Task MessageReceived(MessageReceivedContext context)
        {
            _logger.LogInformation("Scheme {scheme}: MessageReceived called...", context.Scheme.Name);
            return base.MessageReceived(context);
        }

        public override Task TokenValidated(TokenValidatedContext context)
        {
            _logger.LogInformation("Scheme {scheme}: TokenValidated called...", context.Scheme.Name);
            return base.TokenValidated(context);
        }

        public override Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            _logger.LogInformation("Scheme {scheme}: AuthenticationFailed called...", context.Scheme.Name);
            return base.AuthenticationFailed(context);
        }
    }
}
