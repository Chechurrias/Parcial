namespace Domain.Entities
{
    public class Order
    {
        public int Id { get; } // Setter privado eliminado
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // Si este método no tiene lógica útil, elimínalo
        // public void CalculateTotal() { }

        // O si lo necesitas para alguna interfaz, simplemente déjalo vacío (solo si realmente hace falta):
        // public void CalculateTotal() { /* sin lógica, advertencia desactivada */ }
    }
}
