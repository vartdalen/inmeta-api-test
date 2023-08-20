using Microsoft.AspNetCore.Authorization;
using Inmeta.Test.Startup.Authorization.Requirements;

namespace Inmeta.Test.Startup.Authorization.Handlers
{
    public class AdminOrCustomerHashIdRouteValueMatchAuthorizationHandler :
        AuthorizationHandler<AdminOrCustomerHashIdRouteValueMatchRequirement>
    {
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AdminOrCustomerHashIdRouteValueMatchRequirement requirement
        )
        {
            if (context.User.IsInRole("admin") is true || IsCustomerHashIdRouteValueMatch(context))
                context.Succeed(requirement);
            else context.Fail();
            await Task.CompletedTask;
        }

        private static bool IsCustomerHashIdRouteValueMatch(AuthorizationHandlerContext context)
        {
            var httpContext = context.Resource as HttpContext;
            var customerHashIdClaim = context?.User?.FindFirst(x => x.Type == "customer_hash_id")?.Value;
            var customerHashIdRouteValue = httpContext?.Request?.RouteValues["hashId"] as string;
            return customerHashIdClaim == customerHashIdRouteValue;
        }
    }
}