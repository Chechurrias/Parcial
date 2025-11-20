using System;
using System.Linq;
using Xunit;
using Domain.Services;
using Domain.Entities;

namespace Application.Tests // <--- AGREGA ESTE BLOQUE
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
            var order = OrderService.CreateOrder(customer, product, qty, price);
            Assert.NotNull(order);
            Assert.Equal(customer, order.CustomerName);
            Assert.Equal(product, order.ProductName);
            Assert.Equal(qty, order.Quantity);
            Assert.Equal(price, order.UnitPrice);
            Assert.Contains(order, OrderService.LastOrders);
        }

        [Fact]
        public void CreateOrder_Throws_WhenCustomerIsNullOrWhiteSpace()
        {
            Assert.Throws<ArgumentException>(() =>
                OrderService.CreateOrder(null, "P", 1, 1));
            Assert.Throws<ArgumentException>(() =>
                OrderService.CreateOrder("", "P", 1, 1));
            Assert.Throws<ArgumentException>(() =>
                OrderService.CreateOrder("   ", "P", 1, 1));
        }

        [Fact]
        public void CreateOrder_Throws_WhenProductIsNullOrWhiteSpace()
        {
            Assert.Throws<ArgumentException>(() =>
                OrderService.CreateOrder("C", null, 1, 1));
            Assert.Throws<ArgumentException>(() =>
                OrderService.CreateOrder("C", "", 1, 1));
            Assert.Throws<ArgumentException>(() =>
                OrderService.CreateOrder("C", "   ", 1, 1));
        }

        [Fact]
        public void CreateOrder_Throws_WhenQtyIsZeroOrNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                OrderService.CreateOrder("C", "P", 0, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                OrderService.CreateOrder("C", "P", -10, 1));
        }

        [Fact]
        public void CreateOrder_Throws_WhenPriceIsNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                OrderService.CreateOrder("C", "P", 1, -1));
        }

        [Fact]
        public void LastOrders_ReturnsAllCreatedOrders()
        {
            var c = Guid.NewGuid().ToString();
            var p = Guid.NewGuid().ToString();
            var order = OrderService.CreateOrder(c, p, 2, 9.99m);
            Assert.Contains(order, OrderService.LastOrders);
            Assert.True(OrderService.LastOrders.Count > 0);
        }
    }
}
