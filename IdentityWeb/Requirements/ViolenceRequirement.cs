using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IdentityWeb.Requirements
{
    public class ViolenceRequirement : IAuthorizationRequirement
    {
        public int ThresholdAge { get; set; }
    }
    public class ViolenceRequirementHandler : AuthorizationHandler<ViolenceRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViolenceRequirement requirement)
        {

            if (!context.User.HasClaim(c => c.Type == "birthdate"))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            Claim birthDateClaim=context.User.FindFirst(c => c.Type == "birthdate")!;

            if (!DateTime.TryParse(birthDateClaim.Value, out var birthDate))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var age = DateTime.Now.Year - birthDate.Year;
            if (birthDate > DateTime.Now.AddYears(-age)) age--;

            if (age < requirement.ThresholdAge)
            {
                context.Fail();
                return Task.CompletedTask;
            }






            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
