using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ExamManagement.Helper
{
    public static class ProjectHelper
    {
        public static string[] ParseAuthorizationHeader(AuthenticationHeaderValue headerValue)
        {
            string authHeader = null;
            var auth = headerValue;
            if (auth != null && auth.Scheme == "Basic")
                authHeader = auth.Parameter;

            if (string.IsNullOrEmpty(authHeader))
                return null;

            authHeader = Encoding.Default.GetString(Convert.FromBase64String(authHeader));

            var tokens = authHeader.Split(':');
            if (tokens.Length < 2)
                return null;

            return tokens;
        }
    }

}
