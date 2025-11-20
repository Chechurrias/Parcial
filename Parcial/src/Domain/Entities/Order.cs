namespace Domain.Entities
{
    public class Order
    {
        public int Id { get; } // Setter privado eliminado
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public void CalculateTotal()
        {
            var total = Quantity * UnitPrice;
            // Logica de logging eliminada, delegada a Application/Infra
        }
    }
}
