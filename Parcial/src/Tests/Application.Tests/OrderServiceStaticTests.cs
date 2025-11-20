using System;
using System.Linq;
using Xunit;
using DomainOrderService = Domain.Services.OrderService;
using Domain.Entities;

namespace Application.Tests
{
    public class OrderServiceStaticTests
    {
        [Fact]
        public void CreateOrder_ValidArguments_AddsOrder()
        {
            var customer = "Cliente X";
            var product = "Producto Y";
            int qty = 2;
            decimal price = 100;
            var order = DomainOrderService.CreateOrder(customer, product, qty, price);
            Assert.NotNull(order);
            Assert.Equal(customer, order.CustomerName);
            Assert.Equal(product, order.ProductName);
            Assert.Equal(qty, order.Quantity);
            Assert.Equal(price, order.UnitPrice);
            Assert.Contains(order, DomainOrderService.LastOrders);
        }

        [Fact]
        public void CreateOrder_Throws_WhenCustomerIsNullOrWhiteSpace()
        {
            Assert.Throws<ArgumentException>(() =>
                DomainOrderService.CreateOrder(null, "P", 1, 1));
            Assert.Throws<ArgumentException>(() =>
                DomainOrderService.CreateOrder("", "P", 1, 1));
            Assert.Throws<ArgumentException>(() =>
                DomainOrderService.CreateOrder("   ", "P", 1, 1));
        }

        [Fact]
        public void CreateOrder_Throws_WhenProductIsNullOrWhiteSpace()
        {
            Assert.Throws<ArgumentException>(() =>
                DomainOrderService.CreateOrder("C", null, 1, 1));
            Assert.Throws<ArgumentException>(() =>
                DomainOrderService.CreateOrder("C", "", 1, 1));
            Assert.Throws<ArgumentException>(() =>
                DomainOrderService.CreateOrder("C", "   ", 1, 1));
        }

        [Fact]
        public void CreateOrder_Throws_WhenQtyIsZeroOrNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                DomainOrderService.CreateOrder("C", "P", 0, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                DomainOrderService.CreateOrder("C", "P", -10, 1));
        }

        [Fact]
        public void CreateOrder_Throws_WhenPriceIsNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                DomainOrderService.CreateOrder("C", "P", 1, -1));
        }

        [Fact]
        public void LastOrders_ReturnsAllCreatedOrders()
        {
            var c = Guid.NewGuid().ToString();
            var p = Guid.NewGuid().ToString();
            var order = DomainOrderService.CreateOrder(c, p, 2, 9.99m);
            Assert.Contains(order, DomainOrderService.LastOrders);
            Assert.True(DomainOrderService.LastOrders.Count > 0);
        }
    }
}
