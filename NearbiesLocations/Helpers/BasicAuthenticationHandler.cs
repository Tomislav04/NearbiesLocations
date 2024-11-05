using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using NearbiesLocations.Data;
using Microsoft.EntityFrameworkCore;

namespace NearbiesLocations.Helpers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly LocationContext _context;

        public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        LocationContext context)
        : base(options, logger, encoder, clock)
        {
            _context = context;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Provjerava postoji li Authorization header
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                var credentialBytes = Convert.FromBase64String(authHeader.Substring("Basic ".Length).Trim());
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
                var username = credentials[0];
                var apiKey = credentials[1];

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.ApiKey == apiKey);

                if (user == null)
                {
                    throw new Exception();
                }

                // Kreira identitet i tvrdi za korisnika
                var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.AuthenticationInstant, apiKey)
                };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch(Exception ex)
            {
                return AuthenticateResult.Fail(ex);
            }
        }
    }
}
