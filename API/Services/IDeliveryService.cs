using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineFoodDelivery.Models;

namespace OnlineFoodDelivery.Services
{
    public interface IDeliveryService
    {
        Task<List<DeliveryDriver>> GetDriversDescRating(decimal minRating);

    }
}