using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication.Cookies
{
    public class LogCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        private readonly ILogger<LogCookieAuthenticationEvents> _logger;

        public LogCookieAuthenticationEvents(ILogger<LogCookieAuthenticationEvents> logger)
        {
            _logger = logger;
        }

        public override Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            _logger.LogInformation("Scheme {scheme}: ValidatePrincipal called...", context.Scheme.Name);
            return base.ValidatePrincipal(context);
        }

        public override Task SigningIn(CookieSigningInContext context)
        {
            _logger.LogInformation("Scheme {scheme}: SigningIn called...", context.Scheme.Name);
            return base.SigningIn(context);
        }

        public override Task SignedIn(CookieSignedInContext context)
        {
            _logger.LogInformation("Scheme {scheme}: SignedIn called...", context.Scheme.Name);
            return base.SignedIn(context);
        }

        public override Task SigningOut(CookieSigningOutContext context)
        {
            _logger.LogInformation("Scheme {scheme}: SigningOut called...", context.Scheme.Name);
            return base.SigningOut(context);
        }

        public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            _logger.LogInformation("Scheme {scheme}: RedirectToLogin called...", context.Scheme.Name);
            return base.RedirectToLogin(context);
        }

        public override Task RedirectToReturnUrl(RedirectContext<CookieAuthenticationOptions> context)
        {
            _logger.LogInformation("Scheme {scheme}: RedirectToReturnUrl called...", context.Scheme.Name);
            return base.RedirectToReturnUrl(context);
        }

        public override Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            _logger.LogInformation("Scheme {scheme}: RedirectToAccessDenied called...", context.Scheme.Name);
            return base.RedirectToAccessDenied(context);
        }

        public override Task RedirectToLogout(RedirectContext<CookieAuthenticationOptions> context)
        {
            _logger.LogInformation("Scheme {scheme}: RedirectToLogout called...", context.Scheme.Name);
            return base.RedirectToLogout(context);
        }
    }
}
