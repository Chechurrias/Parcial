public class Order
{
    private int _id;
    private string _customerName;
    private string _productName;
    private int _quantity;
    private decimal _unitPrice;

    // Propiedad pública con control completo
    public int Id
    {
        get { return _id; }
        private set { _id = value; } // set privado si solo la clase la modifica
    }

    public string CustomerName
    {
        get { return _customerName; }
        set { _customerName = value; }
    }

    public string ProductName
    {
        get { return _productName; }
        set { _productName = value; }
    }

    public int Quantity
    {
        get { return _quantity; }
        set { _quantity = value; }
    }

    public decimal UnitPrice
    {
        get { return _unitPrice; }
        set { _unitPrice = value; }
    }

    public void CalculateTotalAndLog()
    {
        var total = Quantity * UnitPrice;
        Infrastructure.Logging.Logger.Log($"Total: {total}");
    }
}
