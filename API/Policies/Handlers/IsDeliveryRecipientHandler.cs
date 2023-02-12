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
    public class IsDeliveryRecipientHandler : AuthorizationHandler<IsDeliveryRecipientRequirement, IDeliveryRepository>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsDeliveryRecipientRequirement requirement, IDeliveryRepository deliveryRepo)
        {
            if (context.Resource is AuthorizationFilterContext ctx)
            {
                var deliveryIdString = ctx.HttpContext.GetRouteValue("id").ToString();
                var deliveryId = int.Parse(deliveryIdString);

                var isRecipient = await deliveryRepo.IsDeliveryRecipient(deliveryId, context.User.GetUserId());

                if (isRecipient)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }

}