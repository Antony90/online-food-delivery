using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineFoodDelivery.Extensions;
using OnlineFoodDelivery.Policies.Requirements;
using OnlineFoodDelivery.Repository;

namespace OnlineFoodDelivery.Policies.Handlers
{
    public class IsDeliveryDriverHandler : AuthorizationHandler<IsDeliveryDriverRequirement, IDeliveryRepository>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsDeliveryDriverRequirement requirement, IDeliveryRepository deliveryRepo)
        {
            if (context.Resource is AuthorizationFilterContext ctx)
            {
                var deliveryIdString = ctx.HttpContext.GetRouteValue("id").ToString();
                var deliveryId = int.Parse(deliveryIdString);

                var driverUserId = await deliveryRepo.GetDeliveryDriverUserId(deliveryId);

                if (driverUserId == context.User.GetUserId())
                {
                    context.Succeed(requirement);
                }
            }
        }
    }

}