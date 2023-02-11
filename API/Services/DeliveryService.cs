using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineFoodDelivery.Data;
using OnlineFoodDelivery.Models;

namespace OnlineFoodDelivery.Services
{
    public class DeliveryService : IDeliveryService
    {
        private ApplicationDBContext _context;

        public DeliveryService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<DeliveryDriver>> GetDriversDescRating(decimal minRating)
        {
            return await _context.DeliveryDriver.Where(d => d.AverageRating >= (double)minRating)
                .OrderBy(x => -x.AverageRating) // asc to desc conversion
                .ToListAsync();
        }
    }
}