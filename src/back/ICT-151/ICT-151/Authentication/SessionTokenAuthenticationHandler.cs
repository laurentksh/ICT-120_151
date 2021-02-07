using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ICT_151.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ICT_151.Authentication
{
    public class SessionTokenAuthenticationHandler : AuthenticationHandler<SessionTokenAuthOptions>
    {
        private readonly IUserService UserService;

        public SessionTokenAuthenticationHandler(IOptionsMonitor<SessionTokenAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IUserService userService) : base(options, logger, encoder, clock)
        {
            UserService = userService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(Options.RuntimeTokenHeaderName))
                return AuthenticateResult.Fail($"Missing Header For Token: {Options.RuntimeTokenHeaderName}");
            
            var token = Request.Headers[Options.RuntimeTokenHeaderName];
            
            var session = await UserService.ValidateUserSession(token);


            if (session == null)
                return AuthenticateResult.Fail("Session does not exists.");

            Logger.LogTrace($"Token: {token}; SessionId: {session?.Id}");

            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, session.User.Id.ToString()),
                new Claim(ClaimTypes.Name, session.User.Username),
                new Claim(ClaimTypes.Email, session.User.Email)
            };

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                ExpiresUtc = session.ExpiracyDate,
                IssuedUtc = session.CreationDate,
            };

            var id = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(id);
            var ticket = new AuthenticationTicket(principal, authProperties, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }

    public class SessionTokenAuthOptions : AuthenticationSchemeOptions
    {
        public const string DefaultSchemeName = "SessionTokenAuthenticationHandlerScheme";

        public const string TokenHeaderName = "X-ICT-151-TOKEN";

        public string RuntimeTokenHeaderName { get => TokenHeaderName; }
    }
}
