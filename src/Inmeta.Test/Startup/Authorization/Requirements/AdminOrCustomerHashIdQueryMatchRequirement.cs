using Microsoft.AspNetCore.Authorization;

namespace Inmeta.Test.Startup.Authorization.Requirements
{
    public class AdminOrCustomerHashIdQueryMatchRequirement : IAuthorizationRequirement { }
}