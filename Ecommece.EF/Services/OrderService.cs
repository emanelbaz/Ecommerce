using Ecommece.Core.Interfaces;
using Ecommece.Core.Models;
using Ecommece.Core.Payments;
using Ecommece.Core.Shipping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommece.EF.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBasketRepository _basketRepo;
        private readonly IMessageBroker _broker;

        public OrderService(IOrderRepository orderRepository, IBasketRepository basketRepo, IMessageBroker broker)
        {
            _orderRepository = orderRepository;
            _basketRepo = basketRepo;
            _broker = broker;
        }

        public async Task<(bool Success, string Message, Order? Order)> CreateOrderAsync(CreateOrderRequest request)
        {
            // 1) read basket from redis
            var basket = await _basketRepo.GetBasketAsync(request.BasketId);
            if (basket == null || basket.Items.Count == 0)
                return (false, "Basket is empty", null);

            // 2) get variant ids from basket
            var variantIds = basket.Items.Select(x => x.VariantId).ToList();

            // Start transaction first
            await using var transaction = await _orderRepository.BeginTransactionAsync();

            // 3) load variants with related data using FindAsync (avoids CTE syntax issues)
            var variants = await _orderRepository.GetVariantsByIdsAsync(variantIds);

            if (variants.Count != basket.Items.Count)
                return (false, "Some product variants not found", null);

            // 4) check stock and decrement
            foreach (var item in basket.Items)
            {
                var variant = variants.First(v => v.Id == item.VariantId);
                if (variant.StockQuantity < item.Quantity)
                    return (false, $"Variant {variant.Id} does not have enough stock", null);

                variant.StockQuantity -= item.Quantity;
            }

            // 5) Payment + Shipping + Calculate totals
            var paymentStrategy = PaymentStrategyFactory.GetPaymentStrategy(request.PaymentMethod);
            var shippingStrategy = ShippingFactory.GetShippingStrategy(request.ShippingMethod);

            var total = basket.Items.Sum(i => i.Price * i.Quantity);
            var shippingCost = shippingStrategy.CalculateShippingCost(total);
            var finalTotal = total + shippingCost;

            // 6) Create Order
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
                Items = basket.Items.Select(i =>
                {
                    var variant = variants.First(v => v.Id == i.VariantId);

                    return new OrderItem
                    {
                        ProductVariantId = variant.Id,
                        ProductVariant = variant,
                        ProductName = variant.Product?.Translations.FirstOrDefault(t => t.Language == "en")?.Name ?? "Unknown",
                        PictureUrl = variant.Product?.Pictures.FirstOrDefault()?.PictureUrl,
                        Price = i.Price,
                        Quantity = i.Quantity
                    };
                }).ToList()
            };

            // 7) Add order and save
            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();
            await transaction.CommitAsync();

            // 8) publish payment event
            await _broker.PublishAsync("payments", new
            {
                OrderId = order.Id,
                Amount = finalTotal,
                PaymentMethod = request.PaymentMethod
            });

            // 9) clear basket
            await _basketRepo.DeleteBasketAsync(request.BasketId);

            return (true, "Order created successfully", order);
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
