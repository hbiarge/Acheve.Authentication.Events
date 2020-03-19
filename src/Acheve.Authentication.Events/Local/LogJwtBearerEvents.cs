using System;
using Acheve.Authentication.Events.Local;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication
{
    public class EventsTypeJwtBearerPostConfigureOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        public void PostConfigure(string name, JwtBearerOptions options)
        {
            if (options.EventsType is null)
            {
                options.EventsType = typeof(LogJwtBearerEvents);
            }
            else
            {
                options.EventsType = typeof(LogJwtBearerEvents<>).MakeGenericType(options.EventsType);
            }
        }
    }

    public class LogJwtBearerEvents : JwtBearerEvents
    {
        private readonly IOptionsMonitor<JwtBearerOptions> _options;
        private readonly ILogger<LogJwtBearerEvents> _logger;

        public LogJwtBearerEvents(IOptionsMonitor<JwtBearerOptions> options, ILogger<LogJwtBearerEvents> logger)
        {
            _options = options;
            _logger = logger;
        }

        public override async Task Challenge(JwtBearerChallengeContext context)
        {
            _logger.Challenge(
                scheme: context.Scheme.Name,
                failure: context.AuthenticateFailure,
                error: context.Error,
                errorDescription: context.ErrorDescription);

            await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.Challenge(context));

            await base.Challenge(context);
        }

        public override async Task MessageReceived(MessageReceivedContext context)
        {
            _logger.MessageReceived(
               scheme: context.Scheme.Name);

            await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.MessageReceived(context));

            await base.MessageReceived(context);
        }

        public override async Task TokenValidated(TokenValidatedContext context)
        {
            _logger.TokenValidated(
               scheme: context.Scheme.Name,
               principal: context.Principal,
               token: context.SecurityToken.Id);

            await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.TokenValidated(context));

            await base.TokenValidated(context);
        }

        public override async Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            _logger.AuthenticationFailed(
               scheme: context.Scheme.Name,
               exception: context.Exception);

            await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.AuthenticationFailed(context));

            await base.AuthenticationFailed(context);
        }

        private static async Task SafeCallOriginalEvent(JwtBearerEvents events, Func<JwtBearerEvents, Task> action)
        {
            if (events != null)
            {
                await action(events);
            }
        }
    }

    public class LogJwtBearerEvents<TOther> : LogJwtBearerEvents where TOther : JwtBearerEvents
    {
        private readonly TOther _originalEvents;

        public LogJwtBearerEvents(IOptionsMonitor<JwtBearerOptions> options, TOther originalEvents, ILogger<LogJwtBearerEvents> logger)
        : base(options, logger)
        {
            _originalEvents = originalEvents;
        }

        public override async Task Challenge(JwtBearerChallengeContext context)
        {
            await base.Challenge(context);
            await _originalEvents.Challenge(context);
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

        public override async Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            await base.AuthenticationFailed(context);
            await _originalEvents.AuthenticationFailed(context);
        }
    }
}
