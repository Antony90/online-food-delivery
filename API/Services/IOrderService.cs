using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineFoodDelivery.Dtos.Request;
using OnlineFoodDelivery.Models;

namespace OnlineFoodDelivery.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderFromItems(string userId, CreateOrderDto orderDto, IEnumerable<Item> items);
        Task<EmailResult> EmailReciept(int id);
        Task<Order> ReOrderFromPrevious(int id, CreateOrderDto orderDto);
    }
}