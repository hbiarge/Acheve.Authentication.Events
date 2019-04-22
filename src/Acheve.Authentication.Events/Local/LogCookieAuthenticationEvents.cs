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
        private readonly ILogger<LogCookieAuthenticationEvents> _logger;

        public LogCookieAuthenticationEvents(ILogger<LogCookieAuthenticationEvents> logger)
        {
            _logger = logger;
        }

        public override Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            _logger.ValidatePrincipal(
                scheme: context.Scheme.Name,
                principal: context.Principal);

            return base.ValidatePrincipal(context);
        }

        public override Task SigningIn(CookieSigningInContext context)
        {
            _logger.SigningIn(
                scheme: context.Scheme.Name,
                principal: context.Principal);

            return base.SigningIn(context);
        }

        public override Task SignedIn(CookieSignedInContext context)
        {
            _logger.SignedIn(
                 scheme: context.Scheme.Name,
                 principal: context.Principal);

            return base.SignedIn(context);
        }

        public override Task SigningOut(CookieSigningOutContext context)
        {
            _logger.SigningOut(
                scheme: context.Scheme.Name);

            return base.SigningOut(context);
        }

        public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            _logger.RedirectToLogin(
                scheme: context.Scheme.Name,
                redirectUri: context.RedirectUri);

            return base.RedirectToLogin(context);
        }

        public override Task RedirectToReturnUrl(RedirectContext<CookieAuthenticationOptions> context)
        {
            _logger.RedirectToReturnUrl(
                scheme: context.Scheme.Name,
                redirectUri: context.RedirectUri);

            return base.RedirectToReturnUrl(context);
        }

        public override Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            _logger.RedirectToAccessDenied(
                scheme: context.Scheme.Name,
                redirectUri: context.RedirectUri);

            return base.RedirectToAccessDenied(context);
        }

        public override Task RedirectToLogout(RedirectContext<CookieAuthenticationOptions> context)
        {
            _logger.RedirectToLogout(
                scheme: context.Scheme.Name,
                redirectUri: context.RedirectUri);

            return base.RedirectToLogout(context);
        }
    }
}
