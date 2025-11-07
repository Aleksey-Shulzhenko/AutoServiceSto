using System;

namespace AutoServiceSto.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int CarId { get; set; }
        public string Description { get; set; } = "";
        public decimal Cost { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public Car? Car { get; set; }
        public Client? Client { get; set; }
    }
}