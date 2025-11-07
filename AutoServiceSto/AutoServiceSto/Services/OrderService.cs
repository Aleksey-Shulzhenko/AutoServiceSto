using AutoServiceSto.Data;
using AutoServiceSto.Models;
using System.Collections.Generic;
using System.Linq;

namespace AutoServiceSto.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;
        private readonly FinanceService _finance;

        public OrderService(AppDbContext context, FinanceService finance)
        {
            _context = context;
            _finance = finance;
        }

        public List<Order> GetAll() => _context.Orders.ToList();

        public void Add(string description, int carId, decimal cost)
        {
            var car = _context.Cars.Find(carId);
            if (car == null) return;

            var order = new Order
            {
                CarId = carId,
                ClientId = car.ClientId,
                Description = description,
                Cost = cost
            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            _finance.AddIncome(cost, $"Замовлення #{order.Id}");
        }

        public void Delete(int id)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }
    }
}