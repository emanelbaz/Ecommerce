using Ecommece.Core.Interfaces;
using Ecommece.Core.Models;
using Ecommece.Core.Payments;
using Ecommece.Core.Shipping;
using Ecommece.EF.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.EF.Services
{

    public class OrderService : IOrderService
    {
        private readonly Context _context;
        private readonly IMessageBroker _broker;

        public OrderService(Context context, IMessageBroker broker)
        {
            _context = context;
            _broker = broker;
        }

        public async Task<(bool Success, string Message, Order? Order)> CreateOrderAsync(CreateOrderRequest request)
        {
            var paymentStrategy = PaymentStrategyFactory.GetPaymentStrategy(request.PaymentMethod);
            var shippingStrategy = ShippingFactory.GetShippingStrategy(request.ShippingMethod);

            var total = request.Items.Sum(i => i.Price * i.Quantity);
            var shippingCost = shippingStrategy.CalculateShippingCost(total);
            var finalTotal = total + shippingCost;

            var order = new Order
            {
                UserId = request.UserId,
                Subtotal = finalTotal,
                PaymentMethod = request.PaymentMethod,
                ShippingMethod = request.ShippingMethod,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                Items = request.Items.Select(i => new OrderItem
                {
                    ProductItemId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            // نرسل حدث الدفع إلى الـ Broker
            await _broker.PublishAsync("payments", new
            {
                OrderId = order.Id,
                Amount = finalTotal,
                PaymentMethod = request.PaymentMethod
            });

            return (true, "Order created successfully. Payment is processing...", order);
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.Include(o => o.Items).ToListAsync();
        }
    }

}
