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
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageBroker _broker;

        public OrderService(IOrderRepository orderRepository, IMessageBroker broker)
        {
            _orderRepository = orderRepository;
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
                BuyerEmail = request.BuyerEmail,
                OrderDate = DateTime.UtcNow,
                Subtotal = finalTotal,
                Status = OrderStatus.Pending,
                PaymentMethod = request.PaymentMethod,
                ShippingMethod = request.ShippingMethod,
                ShippingAddress = new Address
                {
                    FirstName = request.ShippingAddress.FirstName,
                    LastName = request.ShippingAddress.LastName,
                    Street = request.ShippingAddress.Street,
                    City = request.ShippingAddress.City,
                    Country = request.ShippingAddress.Country
                },
                Items = request.Items.Select(i => new OrderItem
                {
                    ProductVariantId = i.ProductVariantId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            // إرسال حدث الدفع للـ broker
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
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }
    }

}
