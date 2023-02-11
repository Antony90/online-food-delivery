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
    public class OwnsOrderHandler : AuthorizationHandler<OwnsOrderRequirement, IOrderRepository>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnsOrderRequirement requirement, IOrderRepository orderRepo)
        {
            if (context.Resource is AuthorizationFilterContext ctx)
            {
                var orderIdString = ctx.HttpContext.GetRouteValue("id").ToString();
                var orderId = int.Parse(orderIdString);

                var order = await orderRepo.GetByIdAsync(orderId);

                if (context.User.GetUserId() == order?.UserId)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }

}