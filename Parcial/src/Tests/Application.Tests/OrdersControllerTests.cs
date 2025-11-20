using WebApi.Controllers;
using Xunit;

namespace Application.Tests // <--- AGREGA ESTE BLOQUE
{
    public class OrdersControllerTests
    {
        [Fact]
        public void DoNothing_ReturnsExpectedMessage()
        {
            // Act
            var mensaje = OrdersController.DoNothing();
            // Assert
            Assert.Equal(OrdersController.Message, mensaje);
            Assert.Equal("This controller does nothing. Endpoints are in Program.cs", mensaje);
        }
    }
}
