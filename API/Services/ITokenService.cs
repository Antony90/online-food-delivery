using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using OnlineFoodDelivery.Models;

namespace OnlineFoodDelivery.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}