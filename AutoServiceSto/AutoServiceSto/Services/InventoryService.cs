using AutoServiceSto.Data;
using AutoServiceSto.Models;
using System.Collections.Generic;
using System.Linq;

namespace AutoServiceSto.Services
{
    public class InventoryService
    {
        private readonly AppDbContext _context;
        private readonly FinanceService _financeService;

        public InventoryService(AppDbContext context, FinanceService financeService)
        {
            _context = context;
            _financeService = financeService;
        }

        public List<Part> GetAll() => _context.Parts.ToList();

        public void Add(string name, int quantity, decimal price)
        {
            var part = new Part
            {
                Name = name,
                QuantityInStock = quantity,
                Price = price
            };
            _context.Parts.Add(part);
            _context.SaveChanges();

            _financeService.AddExpense(quantity * price, $"Закупівля: {name}");
        }

        public void PurchasePart(int partId, int quantity)
        {
            var part = _context.Parts.FirstOrDefault(p => p.Id == partId);
            if (part == null) return;

            part.QuantityInStock += quantity;
            _context.SaveChanges();

            _financeService.AddExpense(quantity * part.Price, $"Закупівля {part.Name}");
        }

        public void Delete(int id)
        {
            var part = _context.Parts.Find(id);
            if (part != null)
            {
                _context.Parts.Remove(part);
                _context.SaveChanges();
            }
        }
    }
}