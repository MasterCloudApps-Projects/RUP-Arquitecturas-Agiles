using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ShareThings.Areas.Identity.Data;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ShareThings.FunctionalTest.Authorization
{
    class AuthenticationHandlerTest : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public AuthenticationHandlerTest(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            ShareThingsUser user = AuthorizationFactory.Get(this.Context.Request.Headers);

            var claims = new[] {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var identity = new ClaimsIdentity(claims, user.UserName);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, user.UserName);

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }

    class AuthorizationFactory
    {
        public static ShareThingsUser Get(IHeaderDictionary Headers)
        {
            string headerAuth = Headers["Authorization"];
            ShareThingsUser user = headerAuth == IdentitySingleton.KeyLender ?
                IdentitySingleton.Instance.GetLender() :
                IdentitySingleton.Instance.GetBorrower();

            return user;
        }
    }
}