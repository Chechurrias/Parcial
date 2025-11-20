using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Domain.Services;

using Domain.Entities;

public static class OrderService
{
    private static readonly ConcurrentBag<Order> LastOrdersBag = new ConcurrentBag<Order>();
    public static IReadOnlyCollection<Order> LastOrders => LastOrdersBag;

    public static Order CreateOrder(string customer, string product, int qty, decimal price)
    {
        if (string.IsNullOrWhiteSpace(customer))
            throw new ArgumentException("Customer no puede estar vacío", nameof(customer));
        if (string.IsNullOrWhiteSpace(product))
            throw new ArgumentException("Product no puede estar vacío", nameof(product));
        if (qty <= 0)
            throw new ArgumentOutOfRangeException(nameof(qty), "Cantidad debe ser mayor que cero");
        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Precio no puede ser negativo");

        var o = new Order
        {
            // Ajusta según tipo definido en Order.cs:
            // Id = nuevoIdEntero,
            CustomerName = customer,
            ProductName = product,
            Quantity = qty,
            UnitPrice = price
        };

        LastOrdersBag.Add(o);

        // Elimina la siguiente línea:
        // Infrastructure.Logging.Logger.Log($"Created order {o.Id} for {customer}");

        return o;
    }
}
