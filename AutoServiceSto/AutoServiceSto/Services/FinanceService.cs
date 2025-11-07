using AutoServiceSto.Data;
using AutoServiceSto.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoServiceSto.Services
{
    public class FinanceService
    {
        private readonly AppDbContext _context;

        public FinanceService(AppDbContext context)
        {
            _context = context;
        }

        public List<FinanceRecord> GetAll()
        {
            // Убираем AsNoTracking для простоты
            return _context.FinanceRecords.ToList();
        }

        public decimal GetBalance()
        {
            // Для SQLite выполняем агрегацию на клиенте
            var records = _context.FinanceRecords.ToList();
            return records.Sum(f => f.Amount);
        }

        public void AddIncome(decimal amount, string description)
        {
            var record = new FinanceRecord
            {
                Amount = amount,
                Description = description,
                Date = DateTime.Now
            };
            _context.FinanceRecords.Add(record);
            _context.SaveChanges();
        }

        public void AddExpense(decimal amount, string description)
        {
            var record = new FinanceRecord
            {
                Amount = -amount,
                Description = description,
                Date = DateTime.Now
            };
            _context.FinanceRecords.Add(record);
            _context.SaveChanges();
        }

        public void Add(string description, decimal amount)
        {
            var record = new FinanceRecord
            {
                Description = description,
                Amount = amount,
                Date = DateTime.Now
            };
            _context.FinanceRecords.Add(record);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var record = _context.FinanceRecords.Find(id);
            if (record != null)
            {
                _context.FinanceRecords.Remove(record);
                _context.SaveChanges();
            }
        }
    }
}