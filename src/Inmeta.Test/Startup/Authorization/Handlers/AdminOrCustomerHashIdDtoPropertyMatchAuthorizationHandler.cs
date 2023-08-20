using Microsoft.AspNetCore.Authorization;
using Inmeta.Test.Startup.Authorization.Requirements;
using Inmeta.Test.Services.Models.Abstractions;

namespace Inmeta.Test.Startup.Authorization.Handlers
{
    public class AdminOrCustomerHashIdDtoPropertyMatchAuthorizationHandler :
        AuthorizationHandler<AdminOrCustomerHashIdDtoPropertyMatchRequirement, ICustomerHashedIdentifiable>
    {
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AdminOrCustomerHashIdDtoPropertyMatchRequirement requirement,
            ICustomerHashedIdentifiable dto
        )
        {
            if (context.User.IsInRole("admin") is true || IsCustomerHashIdDtoPropertyMatch(context, dto.CustomerHashId))
                context.Succeed(requirement);
            else context.Fail();
            await Task.CompletedTask;
        }

        private static bool IsCustomerHashIdDtoPropertyMatch(
            AuthorizationHandlerContext context,
            string customerHashIdDtoProperty
        )
        {
            var customerHashIdClaim = context.User.FindFirst(x => x.Type == "customer_hash_id")?.Value;
            return customerHashIdClaim == customerHashIdDtoProperty;
        }
    }
}