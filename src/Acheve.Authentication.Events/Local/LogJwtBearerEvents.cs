using Acheve.Authentication.Events.Local;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication
{
    public class EventsTypeJwtBearerPostConfigureOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        public void PostConfigure(string name, JwtBearerOptions options)
        {
            options.EventsType = typeof(LogJwtBearerEvents);
        }
    }

    public class LogJwtBearerEvents : JwtBearerEvents
    {
        private readonly ILogger<LogJwtBearerEvents> _logger;

        public LogJwtBearerEvents(ILogger<LogJwtBearerEvents> logger)
        {
            _logger = logger;
        }

        public override Task Challenge(JwtBearerChallengeContext context)
        {
            var handler = new StackFrame(1).GetMethod().DeclaringType.Name;

            _logger.Challenge(
                scheme: context.Scheme.Name,
                failure: context.AuthenticateFailure,
                error: context.Error,
                errorDescription: context.ErrorDescription);

            return base.Challenge(context);
        }

        public override Task MessageReceived(MessageReceivedContext context)
        {
            _logger.MessageReceived(
               scheme: context.Scheme.Name);

            return base.MessageReceived(context);
        }

        public override Task TokenValidated(TokenValidatedContext context)
        {
            _logger.TokenValidated(
               scheme: context.Scheme.Name,
               principal: context.Principal,
               token: context.SecurityToken.Id);

            return base.TokenValidated(context);
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
