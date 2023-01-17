using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineFoodDelivery.Dtos.Request;
using OnlineFoodDelivery.Models;

namespace OnlineFoodDelivery.Repository
{
    public interface IAccountRepository
    {
        Task<IAccountDetails> GetPartnerAccountDetails(string userId);
        Task<IAccountDetails> GetUserAccountDetails(string userId);
        Task<IAccountDetails> GetDeliveryDriverAccountDetails(string userId);
        Task<IAccountDetails> GetAdminAccountDetails(string userId);

        Task<List<BasketItem>> GetBasket(string userId);
        Task<List<BasketItem>> UpdateBasket(string userId, UpdateBasketDto updateBasketDto);
        Task ClearBasketAsync(string userId);
    }
}