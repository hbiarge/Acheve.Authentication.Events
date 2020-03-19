using System;
using Acheve.Authentication.Events.Local;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication
{
    public class EventsTypeCookieAuthenticationPostConfigureOptions : IPostConfigureOptions<CookieAuthenticationOptions>
    {
        public void PostConfigure(string name, CookieAuthenticationOptions options)
        {
            options.EventsType = typeof(LogCookieAuthenticationEvents);
        }
    }

    public class LogCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        private readonly IOptionsMonitor<CookieAuthenticationOptions> _options;
        private readonly ILogger<LogCookieAuthenticationEvents> _logger;

        public LogCookieAuthenticationEvents(IOptionsMonitor<CookieAuthenticationOptions> options, ILogger<LogCookieAuthenticationEvents> logger)
        {
            _options = options;
            _logger = logger;
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            _logger.ValidatePrincipal(
                scheme: context.Scheme.Name,
                principal: context.Principal);

            await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.ValidatePrincipal(context));

            await base.ValidatePrincipal(context);
        }

        public override async Task SigningIn(CookieSigningInContext context)
        {
            _logger.SigningIn(
                scheme: context.Scheme.Name,
                principal: context.Principal);

            await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.SigningIn(context));

            await base.SigningIn(context);
        }

        public override async Task SignedIn(CookieSignedInContext context)
        {
            _logger.SignedIn(
                 scheme: context.Scheme.Name,
                 principal: context.Principal);

            await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.SignedIn(context));

            await base.SignedIn(context);
        }

        public override async Task SigningOut(CookieSigningOutContext context)
        {
            _logger.SigningOut(
                scheme: context.Scheme.Name);

            await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.SigningOut(context));

            await base.SigningOut(context);
        }

        public override async Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            _logger.RedirectToLogin(
                scheme: context.Scheme.Name,
                redirectUri: context.RedirectUri);

            await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.RedirectToLogin(context));

            await base.RedirectToLogin(context);
        }

        public override async Task RedirectToReturnUrl(RedirectContext<CookieAuthenticationOptions> context)
        {
            _logger.RedirectToReturnUrl(
                scheme: context.Scheme.Name,
                redirectUri: context.RedirectUri);

            await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.RedirectToReturnUrl(context));

            await base.RedirectToReturnUrl(context);
        }

        public override async Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            _logger.RedirectToAccessDenied(
                scheme: context.Scheme.Name,
                redirectUri: context.RedirectUri);

            await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.RedirectToAccessDenied(context));

            await base.RedirectToAccessDenied(context);
        }

        public override async Task RedirectToLogout(RedirectContext<CookieAuthenticationOptions> context)
        {
            _logger.RedirectToLogout(
                scheme: context.Scheme.Name,
                redirectUri: context.RedirectUri);

            await SafeCallOriginalEvent(_options.Get(context.Scheme.Name).Events, e => e.RedirectToLogout(context));

            await base.RedirectToLogout(context);
        }

        private async Task SafeCallOriginalEvent(CookieAuthenticationEvents events, Func<CookieAuthenticationEvents, Task> action)
        {
            if (events != null)
            {
                await action(events);
            }
        }
    }
}
