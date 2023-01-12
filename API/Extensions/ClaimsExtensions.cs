using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineFoodDelivery.Extensions
{
    /* Lets us search for a user by username */
    public static class ClaimsExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.Claims.SingleOrDefault(x => x.Type.Equals(ClaimTypes.GivenName)).Value;
        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            var userIdString = user.Claims.SingleOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value;

            return userIdString;
        }
    }

}