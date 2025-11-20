namespace Domain.Entities
{
    public class Order
    {
        public int Id { get; private set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public void CalculateTotal()
        {
            var total = Quantity * UnitPrice;
            // Eliminado: Infrastructure.Logging.Logger.Log(...)
            // Si necesitas avisar/loggear, hazlo desde capas superiores (Application, Infrastructure, WebApi)
        }
    }
}
