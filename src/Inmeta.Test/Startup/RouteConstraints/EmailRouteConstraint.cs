using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing.Constraints;

namespace Inmeta.Test.Startup.RouteConstraints
{
    internal sealed partial class EmailRouteConstraint : RegexRouteConstraint
    {
        [GeneratedRegex(@"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$")]
        private static partial Regex EmailRegex();

        public EmailRouteConstraint()
            : base(EmailRegex())
        {
        }
    }
}