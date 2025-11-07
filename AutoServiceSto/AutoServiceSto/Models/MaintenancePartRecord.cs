namespace AutoServiceSto.Models
{
    public class MaintenancePartRecord
    {
        public int Id { get; set; }
        public int MaintenanceId { get; set; }
        public int PartId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public MaintenanceRecord? Maintenance { get; set; }
        public Part? Part { get; set; }
    }
}