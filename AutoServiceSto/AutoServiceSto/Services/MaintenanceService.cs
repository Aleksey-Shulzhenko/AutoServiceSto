using AutoServiceSto.Data;
using AutoServiceSto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace AutoServiceSto.Services
{
    public class MaintenanceService
    {
        private readonly AppDbContext _context;
        private readonly FinanceService _financeService;
        private readonly InventoryService _inventoryService;

        public MaintenanceService(AppDbContext context, FinanceService financeService, InventoryService inventoryService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _financeService = financeService ?? throw new ArgumentNullException(nameof(financeService));
            _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
        }

        public void PerformMaintenance(int carId, string description, List<MaintenancePart> usedParts, decimal laborCost)
        {
            try
            {
                // Проверяем наличие автомобиля
                var car = _context.Cars?.Find(carId);
                if (car == null)
                    throw new InvalidOperationException("Автомобіль не знайдено");

                // Проверяем наличие всех запчастей (если они используются)
                if (usedParts != null && usedParts.Any())
                {
                    foreach (var part in usedParts)
                    {
                        var stockPart = _context.Parts?.Find(part.PartId);
                        if (stockPart == null)
                            throw new InvalidOperationException($"Запчастина не знайдена: {part.PartName}");

                        if (stockPart.QuantityInStock < part.Quantity)
                            throw new InvalidOperationException($"Недостатньо запчастини: {part.PartName}. Доступно: {stockPart.QuantityInStock}");
                    }

                    // Списываем запчасти со склада
                    foreach (var part in usedParts)
                    {
                        var stockPart = _context.Parts?.Find(part.PartId);
                        if (stockPart != null)
                        {
                            stockPart.QuantityInStock -= part.Quantity;
                        }
                    }
                }

                // Рассчитываем общую стоимость
                decimal partsCost = usedParts?.Sum(p => p.TotalCost) ?? 0;
                decimal totalCost = partsCost + laborCost;

                // Создаем заказ на обслуживание
                var order = new Order
                {
                    CarId = carId,
                    ClientId = car.ClientId,
                    Description = $"{description} (Обслуговування)",
                    Cost = totalCost,
                    Date = DateTime.Now
                };
                _context.Orders.Add(order);
                _context.SaveChanges();

                // Добавляем доход от обслуживания
                _financeService.AddIncome(totalCost, $"Обслуговування #{order.Id}");

                // Создаем запись об обслуживании
                var maintenance = new MaintenanceRecord
                {
                    OrderId = order.Id,
                    CarId = carId,
                    Description = description,
                    LaborCost = laborCost,
                    PartsCost = partsCost,
                    TotalCost = totalCost,
                    Date = DateTime.Now
                };
                _context.MaintenanceRecords.Add(maintenance);
                _context.SaveChanges();

                // Записываем использованные запчасти (если они есть)
                if (usedParts != null && usedParts.Any())
                {
                    foreach (var part in usedParts)
                    {
                        var maintenancePart = new MaintenancePartRecord
                        {
                            MaintenanceId = maintenance.Id,
                            PartId = part.PartId,
                            Quantity = part.Quantity,
                            UnitPrice = part.UnitPrice
                        };
                        _context.MaintenancePartRecords.Add(maintenancePart);
                    }
                    _context.SaveChanges();
                }

                MessageBox.Show($"Обслуговування успішно виконано!\nСтворено замовлення #{order.Id}", "Успіх",
                    MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Помилка під час обслуговування: {ex.Message}", ex);
            }
        }

        public List<MaintenanceRecord> GetAllMaintenanceRecords()
        {
            try
            {
                return _context.MaintenanceRecords?
                    .OrderByDescending(m => m.Date)
                    .ToList() ?? new List<MaintenanceRecord>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження записів обслуговування: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return new List<MaintenanceRecord>();
            }
        }

        public List<Part> GetAvailableParts()
        {
            try
            {
                return _context.Parts?
                    .Where(p => p.QuantityInStock > 0)
                    .OrderBy(p => p.Name)
                    .ToList() ?? new List<Part>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження запчастин: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return new List<Part>();
            }
        }
    }
}