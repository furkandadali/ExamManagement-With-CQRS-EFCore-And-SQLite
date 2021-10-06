using Entities.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ExamManagement.Helper
{
    public class Authorization : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public ExamManagementContext _context;
        public Authorization(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ExamManagementContext context) : base(options, logger, encoder, clock)
        {
            _context = context;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                if (Request.RouteValues.TryGetValue("controller", out object controllerObj) && controllerObj?.ToString() == "HealthCheck")
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, "") };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return AuthenticateResult.Success(ticket);
                }
                if (!Request.Headers.ContainsKey("Authorization"))
                {
                    return AuthenticateResult.Fail("Authorization Header Was Not Found!");
                }
                else
                {
                    var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                    Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                    string usernamePassword = encoding.GetString(Convert.FromBase64String(authenticationHeaderValue.Parameter));

                    int seperatorIndex = usernamePassword.IndexOf(':');
                    var username = usernamePassword.Substring(0, seperatorIndex);
                    var password = usernamePassword.Substring(seperatorIndex + 1);
                    var allUser = _context.ApiUsers.ToList();
                    var apiUser = _context.ApiUsers.FirstOrDefault(m => m.IsDeleted == false && m.ApiKey == username && m.ApiPassword == password);

                    if (apiUser != null)
                    {
                        var claims = new[] { new Claim(ClaimTypes.Name, apiUser.ApiKey) };
                        var identity = new ClaimsIdentity(claims, Scheme.Name);
                        var principal = new ClaimsPrincipal(identity);
                        var ticket = new AuthenticationTicket(principal, Scheme.Name);

                        return AuthenticateResult.Success(ticket);
                    }
                    else
                    {
                        return AuthenticateResult.Fail("Invalid Username Or Password!");

                    }
                }
            }
            catch (Exception)
            {
                return AuthenticateResult.Fail("Error Has Occured!");
            }
        }
    }
}
