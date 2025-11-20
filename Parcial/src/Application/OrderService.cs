using Application.Interfaces;
using Domain.Entities;

namespace Application
{
    public class OrderService : IOrderService
    {
        public Order CreateOrder(string customer, string product, int qty, decimal price)
        {
            return new Order
            {
                CustomerName = customer,
                ProductName = product,
                Quantity = qty,
                UnitPrice = price
            };
        }
    }
}
