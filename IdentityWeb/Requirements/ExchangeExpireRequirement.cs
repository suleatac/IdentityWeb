using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IdentityWeb.Requirements
{
    public class ExchangeExpireRequirement:IAuthorizationRequirement
    {
    }


    public class ExchangeExpireRequirementHandler : AuthorizationHandler<ExchangeExpireRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ExchangeExpireRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "ExchangeExpireDate"))
            {
                context.Fail();
                return Task.CompletedTask; 
            }
            Claim exchangeExpireDate= context.User.FindFirst(c => c.Type == "ExchangeExpireDate")!;

            if (DateTime.TryParse(exchangeExpireDate.Value, out var expireDate))
            {
                if (expireDate < DateTime.Now)
                {
                    context.Fail();
                    return Task.CompletedTask;
                }
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }


}
