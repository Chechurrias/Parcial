using System;
using Domain.Entities;
using Domain.Services;
using Infrastructure.Data;
using Infrastructure.Logging;

namespace Application.UseCases
{
    public class CreateOrderUseCase
    {
        private readonly IOrderService _orderService;
        private readonly IDatabase _database;
        private readonly ILogger _logger;

        // Inyección de dependencias para mejorar testabilidad y desacoplamiento
        public CreateOrderUseCase(IOrderService orderService, IDatabase database, ILogger logger)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Order Execute(string customer, string product, int qty, decimal price)
        {
            _logger.Log("CreateOrderUseCase starting");

            // Suponiendo que CreateOrder es una creación segura y correcta
            var order = _orderService.CreateOrder(customer, product, qty, price);

            // Usar parámetros para evitar SQL Injection
            string sql = "INSERT INTO Orders(Id, Customer, Product, Qty, Price) VALUES (@Id, @Customer, @Product, @Qty, @Price)";

            try
            {
                _database.ExecuteNonQuery(sql, new
                {
                    Id = order.Id,
                    Customer = customer,
                    Product = product,
                    Qty = qty,
                    Price = price
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la orden en la base de datos");
                throw new InvalidOperationException("Error específico al guardar la orden", ex);
            }                       

            // Retirar Sleep; en caso de necesitar esperas, usar asincronía y await Task.Delay()

            return order;
        }
    }
}
