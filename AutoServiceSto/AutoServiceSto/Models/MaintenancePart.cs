namespace AutoServiceSto.Models
{
    public class MaintenancePart
    {
        public int PartId { get; set; }
        public string PartName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalCost => Quantity * UnitPrice;
    }
}