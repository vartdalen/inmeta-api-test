using Microsoft.AspNetCore.Authorization;
using Inmeta.Test.Startup.Authorization.Requirements;

namespace Inmeta.Test.Startup.Authorization.Handlers
{
    public class AdminOrCustomerHashIdQueryMatchAuthorizationHandler :
        AuthorizationHandler<AdminOrCustomerHashIdQueryMatchRequirement>
    {
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AdminOrCustomerHashIdQueryMatchRequirement requirement
        )
        {
            if (context.User.IsInRole("admin") is true || IsCustomerHashIdQueryMatch(context))
                context.Succeed(requirement);
            else context.Fail();
            await Task.CompletedTask;
        }

        private static bool IsCustomerHashIdQueryMatch(AuthorizationHandlerContext context)
        {
            var httpContext = context.Resource as HttpContext;
            var customerHashIdClaim = context?.User?.FindFirst(x => x.Type == "customer_hash_id")?.Value;
            var customerHashIdRouteValue = httpContext?.Request?.Query["q"].ToString();
            return customerHashIdClaim == customerHashIdRouteValue;
        }
    }
}