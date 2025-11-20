namespace Domain.Entities
{
    public class Order
    {
        public int Id { get; private set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public void CalculateTotalAndLog()
        {
            var total = Quantity * UnitPrice;
            Infrastructure.Logging.Logger.Log($"Total: {total}");
        }
    }
}
