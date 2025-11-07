namespace AutoServiceSto.Models
{
    public class Part
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int QuantityInStock { get; set; }
        public decimal Price { get; set; }
    }
}