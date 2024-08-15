namespace BlueDragon.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class InventoryAudit
    {
        public List<Hardware>? Hardwares { get; set; }
        public List<Cable>? Cables { get; set; }
        public List<Ecomponent>? Ecomponents { get; set; }
        public List<Peripheral>? Peripherals { get; set; }
    }
}
