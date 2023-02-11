using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineFoodDelivery.Policies.Requirements;
using OnlineFoodDelivery.Repository;

namespace OnlineFoodDelivery.Policies.Handlers
{
    public class OwnsRestaurantHandler : AuthorizationHandler<OwnsRestaurantRequirement, IRestaurantRepository>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnsRestaurantRequirement requirement, IRestaurantRepository restaurantRepo)
        {
            if (context.Resource is AuthorizationFilterContext ctx)
            {
                var restaurantIdString = ctx.HttpContext.GetRouteValue("id").ToString();
                var restaurantId = int.Parse(restaurantIdString);

                var restaurant = await restaurantRepo.GetByIdAsync(restaurantId);

                // if (context.User.GetUserId() == restaurant.Partner.UserId)
                // {
                context.Succeed(requirement);
                // }
            }
        }
    }

}