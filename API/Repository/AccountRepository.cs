using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineFoodDelivery.Data;
using OnlineFoodDelivery.Dtos.Request;
using OnlineFoodDelivery.Models;

namespace OnlineFoodDelivery.Repository
{
    public class AccountRepository : IAccountRepository
    {

        private readonly ApplicationDBContext _context;

        public AccountRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        /* Basket is attached to the user model since its a 1-1 and doesn't need a separate table */
        public async Task<List<BasketItem>> GetBasket(string userId)
        {
            // var user = await _context.Users.Include(u => u.Basket)
            //     .FirstOrDefaultAsync(u => u.Id == userId) 
            //     ?? throw new Exception("Referenced user does not exist.");
            return await _context.BasketItem.Where(bi => bi.UserId == userId).ToListAsync();
            // return user.Basket.ToList();
        }

        public Task<IAccountDetails> GetAdminAccountDetails(string userId)
        {
            // No admin specific details yet
            throw new NotImplementedException();
        }


        public async Task<IAccountDetails> GetDeliveryDriverAccountDetails(string userId)
        {
            return await _context.DeliveryDriver.FirstOrDefaultAsync(dd => dd.UserId == userId);
        }

        public async Task<IAccountDetails> GetPartnerAccountDetails(string userId)
        {
            return await _context.PartnerUser.FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<IAccountDetails> GetUserAccountDetails(string userId)
        {
            return await _context.Users.FindAsync(userId);
        }


        /* Supports operations:
            - Add new item
            - Update existing quantity
            - Remove item 
        */
        public async Task<List<BasketItem>> UpdateBasket(string userId, UpdateBasketDto updateDto)
        {
            var basketItem = await _context.BasketItem.FirstOrDefaultAsync(
                bItem => bItem.UserId == userId && bItem.ProductId == updateDto.ProductId);

            // Checks for consistency with AddItem and the db state 
            if (basketItem == null)
            {
                if (updateDto.AddItem ?? false)
                {
                    // Add new item since not in basket
                    _context.BasketItem.Add(new BasketItem
                    {
                        UserId = userId,
                        ProductId = updateDto.ProductId,
                        Quantity = updateDto.Quantity ?? 1,
                    });
                }
                else
                {
                    throw new Exception("Cannot remove item as it is not in the basket.");
                }
            }
            else
            {
                if (updateDto.AddItem ?? false)
                {
                    throw new Exception("Cannot add item as it is already in the basket.");
                }
                else if (updateDto.Quantity != null)
                {
                    // Item exists, not adding an item and quantity is supplied
                    // Therefore update quantity
                    basketItem.Quantity = (int)updateDto.Quantity; // cast needed for compiler even though we just checked if not null
                }
                else
                {
                    // Remove existing item
                    _context.BasketItem.Remove(basketItem);
                }
            }

            await _context.SaveChangesAsync();

            // Updated basket
            return await _context.BasketItem.Where(bItem => bItem.UserId == userId)
                .ToListAsync();

        }

        public async Task ClearBasketAsync(string userId)
        {
            var itemsToRemove = _context.BasketItem.Where(bItem => bItem.UserId == userId);

            _context.BasketItem.RemoveRange(itemsToRemove);
            await _context.SaveChangesAsync();
        }
    }
}