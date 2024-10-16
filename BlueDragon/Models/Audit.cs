namespace BlueDragon.Models;
public class InventoryAudit
{
    public List<Hardware>? Hardwares { get; set; }
    public List<Cable>? Cables { get; set; }
    public List<Ecomponent>? Ecomponents { get; set; }
    public List<Peripheral>? Peripherals { get; set; }
}

public class BarcodeInformation
{
    public string? Prefix { get; set; }
    public string? Suffix { get; set; }
    public string? Plus { get; set; }
    public DateOnly Date { get; set; }
    public string? FullBarcode { get; set; }
    public int Count { get; set; }
}