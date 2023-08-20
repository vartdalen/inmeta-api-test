using Inmeta.Test.Services.Abstractions;
using Inmeta.Test.Services.Models.Dtos.Indexes;
using Inmeta.Test.Startup.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Inmeta.Test.Startup.Authorization.Handlers
{
	public class AdminOrOrderHashIdRouteValueMatchAuthorizationHandler :
        AuthorizationHandler<AdminOrOrderHashIdRouteValueMatchRequirement>
    {
        private readonly ICustomerService _customerService;

        public AdminOrOrderHashIdRouteValueMatchAuthorizationHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AdminOrOrderHashIdRouteValueMatchRequirement requirement
        )
        {
            if (context.User.IsInRole("admin") is true || await IsOrderHashIdRouteValueMatch(context))
                context.Succeed(requirement);
            else context.Fail();
            await Task.CompletedTask;
        }

        private async Task<bool> IsOrderHashIdRouteValueMatch(AuthorizationHandlerContext context)
        {
            var httpContext = context.Resource as HttpContext;
            var customerHashIdClaim = context.User.FindFirst(x => x.Type == "customer_hash_id")!.Value;
            var orderHashIdRouteValue = httpContext?.Request.RouteValues["hashId"] as string;
            try
            {
                return await _customerService.IsResourceOwner(
                    new CustomerHashId(customerHashIdClaim),
                    new OrderHashId(orderHashIdRouteValue!)
                );
            }
            catch (KeyNotFoundException) { return false; }
        }
    }
}