using Xunit;
using Application;
using Domain.Entities;

namespace Application.Tests // <--- AGREGA ESTE BLOQUE
{
    public class OrderServiceTests
    {
        [Fact]
        public void CreateOrder_ReturnsOrder_WithCorrectProperties()
        {
            // Arrange
            var service = new OrderService();
            // Act
            var order = service.CreateOrder("Cliente1", "ProductoA", 3, 25.5m);
            // Assert
            Assert.NotNull(order);
            Assert.Equal("Cliente1", order.CustomerName);
            Assert.Equal("ProductoA", order.ProductName);
            Assert.Equal(3, order.Quantity);
            Assert.Equal(25.5m, order.UnitPrice);
        }
        [Fact]
        public void CreateOrder_Allows_ZeroOrNegativePrice()
        {
            // Arrange
            var service = new OrderService();
            // Act
            var order = service.CreateOrder("TestUser", "ItemZero", 1, 0m);
            // Assert
            Assert.Equal(0m, order.UnitPrice);
            var orderNeg = service.CreateOrder("TestUser", "ItemNegative", 1, -10m);
            Assert.Equal(-10m, orderNeg.UnitPrice);
        }
        [Fact]
        public void CreateOrder_Allows_NegativeOrZeroQuantity()
        {
            // Arrange
            var service = new OrderService();
            var orderZero = service.CreateOrder("X", "Prod", 0, 20m);
            Assert.Equal(0, orderZero.Quantity);
            var orderNeg = service.CreateOrder("Y", "Prod", -5, 20m);
            Assert.Equal(-5, orderNeg.Quantity);
        }
    }
}
