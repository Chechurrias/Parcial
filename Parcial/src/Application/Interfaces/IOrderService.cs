using Domain.Entities;

namespace Application.Interfaces
{
    public interface IOrderService
    {
        Order CreateOrder(string customer, string product, int qty, decimal price);
    }
}
