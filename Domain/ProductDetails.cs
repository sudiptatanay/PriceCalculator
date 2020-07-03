
public class ProductDetails
{
    public Product[] Products { get; set; }
}

public class Product
{
    public string name { get; set; }
    public string unit { get; set; }
    public float price { get; set; }
    public int qty { get; set; }
    public bool has_offer { get; set; }
    public string on_product { get; set; }
    public int on_qty { get; set; }
    public float multiplier { get; set; }
    public float adder { get; set; }
}

