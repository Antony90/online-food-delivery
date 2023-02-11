using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineFoodDelivery.Dtos.Request;
using OnlineFoodDelivery.Models;

namespace OnlineFoodDelivery.Repository
{
    public interface IDeliveryRepository : IGenericRepository<Delivery, CreateDeliveryDto, UpdateDeliveryDto>
    {
        Task<List<Delivery>> GetAllAsync(string userId);
        Task SetUserRating(int deliveryId, int userRating);
        Task<Delivery> CreateAsync(string userId, int orderId, CreateDeliveryDto newObjDto);
        Task<bool> IsDeliveryRecipient(int deliveryId, string userId);
        Task<string?> GetDeliveryDriverUserId(int deliveryId);
    }
}