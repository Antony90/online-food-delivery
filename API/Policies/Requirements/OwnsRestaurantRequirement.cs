using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace OnlineFoodDelivery.Policies.Requirements
{
    public class OwnsRestaurantRequirement : IAuthorizationRequirement
    {
        public OwnsRestaurantRequirement() { }


    }
}