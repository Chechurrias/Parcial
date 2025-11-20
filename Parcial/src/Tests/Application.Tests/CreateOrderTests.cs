using Xunit;
using Moq;
using Application.UseCases;
using Application.Interfaces;
using Domain.Entities;

public class CreateOrderTests
{
    [Fact]
    public void Execute_CreatesAndPersistsOrder()
    {
        // Arrange (configuración de mocks)
        var order = new Order
        {
            CustomerName = "Demo",
            ProductName = "TestItem",
            Quantity = 2,
            UnitPrice = 10m
        };

        var serviceMock = new Mock<IOrderService>();
        serviceMock.Setup(x => x.CreateOrder("Demo", "TestItem", 2, 10m)).Returns(order);

        var dbMock = new Mock<IDatabase>();
        var loggerMock = new Mock<ILogger>();

        var useCase = new CreateOrder(serviceMock.Object, dbMock.Object, loggerMock.Object);

        // Act (ejecución real)
        var result = useCase.Execute("Demo", "TestItem", 2, 10m);

        // Assert (verificaciones)
        Assert.Equal(order, result);
        dbMock.Verify(x => x.ExecuteNonQuery(It.IsAny<string>(), It.IsAny<object>()), Times.Once());
        loggerMock.Verify(x => x.Log(It.IsAny<string>()), Times.Once());
    }
}
