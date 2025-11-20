using System;
using System.Data.SqlClient;
using Domain.Entities;
using Application.Interfaces;

namespace Application.UseCases
{
    public class CreateOrder
    {
        private readonly IOrderService _orderService;
        private readonly IDatabase _database;
        private readonly ILogger _logger;

        public CreateOrder(IOrderService orderService, IDatabase database, ILogger logger)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Order Execute(string customer, string product, int qty, decimal price)
        {
            _logger.Log("CreateOrder starting");

            var order = _orderService.CreateOrder(customer, product, qty, price);

            string sql = "INSERT INTO Orders(Id, Customer, Product, Qty, Price) VALUES (@Id, @Customer, @Product, @Qty, @Price)";
            var parameters = new[]
            {
                new SqlParameter("@Id", order.Id),
                new SqlParameter("@Customer", customer),
                new SqlParameter("@Product", product),
                new SqlParameter("@Qty", qty),
                new SqlParameter("@Price", price)
            };

            try
            {
                _database.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la orden en la base de datos");
                throw new InvalidOperationException("Error específico al guardar la orden", ex);
            }

            return order;
        }
    }
}
