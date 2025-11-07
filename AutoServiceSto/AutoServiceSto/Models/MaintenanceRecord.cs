using System;

namespace AutoServiceSto.Models
{
    public class MaintenanceRecord
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int CarId { get; set; }
        public string Description { get; set; } = "";
        public decimal LaborCost { get; set; }
        public decimal PartsCost { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public Car? Car { get; set; }
        public Order? Order { get; set; }
    }
}