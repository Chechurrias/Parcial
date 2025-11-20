using System;

// BAD: Mixing minimal APIs with Controllers folder just to confuse structure
namespace WebApi.Controllers
{
    public static class OrdersController
    {
        // Constante para el valor fijo
        public const string Message = "This controller does nothing. Endpoints are in Program.cs";

        // Método estático si aún se requiere método (opcional)
        public static string DoNothing() => Message;
    }
}
