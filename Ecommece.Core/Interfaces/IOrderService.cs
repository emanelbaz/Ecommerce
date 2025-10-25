using Ecommece.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Interfaces
{
    public interface IOrderService
    {
        Task<(bool Success, string Message, Order? Order)> CreateOrderAsync(CreateOrderRequest request);
        Task<Order?> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
    }
}
