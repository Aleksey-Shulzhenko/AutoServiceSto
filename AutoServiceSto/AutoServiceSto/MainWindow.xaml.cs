using AutoServiceSto.Data;
using AutoServiceSto.Helpers;
using AutoServiceSto.Models;
using AutoServiceSto.Services;
using AutoServiceSto.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AutoServiceSto
{
    public partial class MainWindow : Window
    {
        private readonly AppDbContext? _db;
        private readonly ClientService? _clientService;
        private readonly CarService? _carService;
        private readonly FinanceService? _financeService;
        private readonly OrderService? _orderService;
        private readonly InventoryService? _inventoryService;
        private readonly MaintenanceService? _maintenanceService;

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                // Подписываемся на события кликов всех кнопок
                SubscribeToButtonClicks();

                // Создаем контекст без параметров
                _db = new AppDbContext();

                // Убедимся, что база создана
                _db.Database.EnsureCreated();

                _clientService = new ClientService(_db);
                _carService = new CarService(_db);
                _financeService = new FinanceService(_db);
                _orderService = new OrderService(_db, _financeService);
                _inventoryService = new InventoryService(_db, _financeService);
                _maintenanceService = new MaintenanceService(_db, _financeService, _inventoryService);

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка ініціалізації: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                SoundPlayerService.PlayErrorSound();
            }
        }

        private void SubscribeToButtonClicks()
        {
            // Находим все кнопки и подписываемся на события
            var buttons = FindVisualChildren<Button>(this);
            foreach (var button in buttons)
            {
                button.Click += (s, e) =>
                {
                    SoundPlayerService.PlayClickSound();
                    ButtonAnimationHelper.PlayClickAnimation(button);
                };
            }
        }

        // Вспомогательный метод для поиска всех кнопок
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    if (child != null)
                    {
                        foreach (T childOfChild in FindVisualChildren<T>(child))
                        {
                            yield return childOfChild;
                        }
                    }
                }
            }
        }

        private void LoadData()
        {
            try
            {
                ClientsGrid.ItemsSource = _clientService?.GetAll() ?? new List<Client>();
                CarsGrid.ItemsSource = _carService?.GetAll() ?? new List<Car>();
                OrdersGrid.ItemsSource = _orderService?.GetAll() ?? new List<Order>();
                PartsGrid.ItemsSource = _inventoryService?.GetAll() ?? new List<Part>();
                FinanceGrid.ItemsSource = _financeService?.GetAll() ?? new List<FinanceRecord>();
                BalanceText.Text = _financeService?.GetBalance().ToString("C2") ?? "0.00 ₴";
                MaintenanceGrid.ItemsSource = _maintenanceService?.GetAllMaintenanceRecords() ?? new List<MaintenanceRecord>();

                // Оновлюємо звіти
                RefreshReports();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження даних: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                SoundPlayerService.PlayErrorSound();
            }
        }

        // === КЛІЄНТИ ===
        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var window = new SimpleInputWindow(new List<string> { "ПІБ", "Телефон", "Email", "Адреса" });
                if (window.ShowDialog() == true)
                {
                    var values = window.Values;
                    _clientService?.Add(values[0], values[1], values[2], values.Count > 3 ? values[3] : "");
                    LoadData();
                    SoundPlayerService.PlaySuccessSound();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка додавання клієнта: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                SoundPlayerService.PlayErrorSound();
            }
        }

        private void EditClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ClientsGrid.SelectedItem is Client client)
                {
                    var window = new EditClientWindow(client);
                    if (window.ShowDialog() == true)
                    {
                        _clientService?.Update(client.Id,
                            window.EditedClient.FullName,
                            window.EditedClient.Phone,
                            window.EditedClient.Email,
                            window.EditedClient.Address);
                        LoadData();
                        SoundPlayerService.PlaySuccessSound();
                    }
                }
                else
                {
                    MessageBox.Show("Виберіть клієнта для редагування", "Інформація",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка редагування клієнта: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                SoundPlayerService.PlayErrorSound();
            }
        }

        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ClientsGrid.SelectedItem is Client client)
                {
                    var result = MessageBox.Show($"Видалити клієнта {client.FullName}?", "Підтвердження",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        _clientService?.Delete(client.Id);
                        LoadData();
                        SoundPlayerService.PlaySuccessSound();
                    }
                }
                else
                {
                    MessageBox.Show("Виберіть клієнта для видалення", "Інформація",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка видалення клієнта: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                SoundPlayerService.PlayErrorSound();
            }
        }

        // === АВТОМОБІЛІ ===
        private void AddCar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_carService != null)
                {
                    var window = new AddCarWindow(_carService);
                    if (window.ShowDialog() == true)
                    {
                        LoadData();
                        SoundPlayerService.PlaySuccessSound();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка додавання автомобіля: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                SoundPlayerService.PlayErrorSound();
            }
        }

        private void EditCar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CarsGrid.SelectedItem is Car car)
                {
                    var window = new EditCarWindow(car);
                    if (window.ShowDialog() == true)
                    {
                        _carService?.UpdateCar(window.EditedCar);
                        LoadData();
                        SoundPlayerService.PlaySuccessSound();

                        MessageBox.Show("Автомобіль успішно оновлено!", "Успіх",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Виберіть автомобіль для редагування", "Інформація",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка редагування автомобіля: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                SoundPlayerService.PlayErrorSound();
            }
        }

        private void DeleteCar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CarsGrid.SelectedItem is Car car)
                {
                    var result = MessageBox.Show($"Видалити автомобіль {car.Brand} {car.Model}?", "Підтвердження",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        _carService?.Delete(car.Id);
                        LoadData();
                        SoundPlayerService.PlaySuccessSound();
                    }
                }
                else
                {
                    MessageBox.Show("Виберіть автомобіль для видалення", "Інформація",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка видалення автомобіля: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                SoundPlayerService.PlayErrorSound();
            }
        }

        // === ЗАМОВЛЕННЯ ===
        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var window = new SimpleInputWindow(new List<string> { "Опис", "ID автомобіля", "Вартість" });
                if (window.ShowDialog() == true)
                {
                    var v = window.Values;
                    if (int.TryParse(v[1], out int carId) && decimal.TryParse(v[2], out decimal cost))
                    {
                        _orderService?.Add(v[0], carId, cost);
                        LoadData();
                        SoundPlayerService.PlaySuccessSound();
                    }
                    else
                    {
                        MessageBox.Show("Невірний формат даних", "Помилка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        SoundPlayerService.PlayErrorSound();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка додавання замовлення: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                SoundPlayerService.PlayErrorSound();
            }
        }

        private void EditOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (OrdersGrid.SelectedItem is Order order)
                {
                    var window = new EditOrderWindow(order);
                    if (window.ShowDialog() == true)
                    {
                        // Для заказов нужно обновить данные в базе
                        _db?.SaveChanges();
                        LoadData();
                        SoundPlayerService.PlaySuccessSound();

                        MessageBox.Show("Замовлення успішно оновлено!", "Успіх",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Виберіть замовлення для редагування", "Інформація",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка редагування замовлення: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                SoundPlayerService.PlayErrorSound();
            }
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (OrdersGrid.SelectedItem is Order order)
                {
                    var result = MessageBox.Show($"Видалити замовлення #{order.Id}?", "Підтвердження",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        _orderService?.Delete(order.Id);
                        LoadData();
                        SoundPlayerService.PlaySuccessSound();
                    }
                }
                else
                {
                    MessageBox.Show("Виберіть замовлення для видалення", "Інформація",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка видалення замовлення: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                SoundPlayerService.PlayErrorSound();
            }
        }

        // === СКЛАД ===
        private void AddPart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var window = new SimpleInputWindow(new List<string> { "Назва", "Кількість", "Ціна" });
                if (window.ShowDialog() == true)
                {
                    var v = window.Values;
                    if (int.TryParse(v[1], out int quantity) && decimal.TryParse(v[2], out decimal price))
                    {
                        _inventoryService?.Add(v[0], quantity, price);
                        LoadData();
                        SoundPlayerService.PlaySuccessSound();
                    }
                    else
                    {
                        MessageBox.Show("Невірний формат даних", "Помилка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        SoundPlayerService.PlayErrorSound();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка додавання запчастини: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                SoundPlayerService.PlayErrorSound();
            }
        }

        private void EditPart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PartsGrid.SelectedItem is Part part)
                {
                    var window = new EditPartWindow(part);
                    if (window.ShowDialog() == true)
                    {
                        // Для запчастей нужно обновить данные в базе
                        _db?.SaveChanges();
                        LoadData();
                        SoundPlayerService.PlaySuccessSound();

                        MessageBox.Show("Запчастину успішно оновлено!", "Успіх",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Виберіть запчастину для редагування", "Інформація",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка редагування запчастини: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                SoundPlayerService.PlayErrorSound();
            }
        }

        private void PurchasePart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PartsGrid.SelectedItem is Part part)
                {
                    var window = new SimpleInputWindow(new List<string> { "Кількість для закупівлі" });
                    if (window.ShowDialog() == true)
                    {
                        var v = window.Values;
                        if (int.TryParse(v[0], out int quantity))
                        {
                            _inventoryService?.PurchasePart(part.Id, quantity);
                            LoadData();
                            SoundPlayerService.PlaySuccessSound();
                        }
                        else
                        {
                            MessageBox.Show("Невірний формат кількості", "Помилка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            SoundPlayerService.PlayErrorSound();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Виберіть запчастину для закупівлі", "Інформація",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка закупівлі: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                SoundPlayerService.PlayErrorSound();
            }
        }

        private void DeletePart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PartsGrid.SelectedItem is Part part)
                {
                    var result = MessageBox.Show($"Видалити запчастину {part.Name}?", "Підтвердження",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        _inventoryService?.Delete(part.Id);
                        LoadData();
                        SoundPlayerService.PlaySuccessSound();
                    }
                }
                else
                {
                    MessageBox.Show("Виберіть запчастину для видалення", "Інформація",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка видалення запчастини: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                SoundPlayerService.PlayErrorSound();
            }
        }

        // === ФІНАНСИ ===
        private void AddIncome_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var window = new SimpleInputWindow(new List<string> { "Опис", "Сума" });
                if (window.ShowDialog() == true)
                {
                    var v = window.Values;
                    if (decimal.TryParse(v[1], out decimal amount))
                    {
                        _financeService?.AddIncome(amount, v[0]);
                        LoadData();
                        SoundPlayerService.PlaySuccessSound();
                    }
                    else
                    {
                        MessageBox.Show("Невірний формат суми", "Помилка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        SoundPlayerService.PlayErrorSound();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка додавання доходу: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                SoundPlayerService.PlayErrorSound();
            }
        }

        private void AddExpense_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var window = new SimpleInputWindow(new List<string> { "Опис", "Сума" });
                if (window.ShowDialog() == true)
                {
                    var v = window.Values;
                    if (decimal.TryParse(v[1], out decimal amount))
                    {
                        _financeService?.AddExpense(amount, v[0]);
                        LoadData();
                        SoundPlayerService.PlaySuccessSound();
                    }
                    else
                    {
                        MessageBox.Show("Невірний формат суми", "Помилка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        SoundPlayerService.PlayErrorSound();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка додавання витрати: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                SoundPlayerService.PlayErrorSound();
            }
        }

        // === ОБСЛУГОВУВАННЯ ===
        private void PerformMaintenance_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_carService == null || _maintenanceService == null)
                {
                    MessageBox.Show("Сервіси не ініціалізовані", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    SoundPlayerService.PlayErrorSound();
                    return;
                }

                var maintenanceWindow = new MaintenanceWindow(_carService, _maintenanceService);
                if (maintenanceWindow.ShowDialog() == true)
                {
                    LoadData();
                    SoundPlayerService.PlaySuccessSound();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка виконання обслуговування: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                SoundPlayerService.PlayErrorSound();
            }
        }

        private void ShowMaintenanceDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MaintenanceGrid.SelectedItem is MaintenanceRecord maintenance)
                {
                    string partsInfo = maintenance.PartsCost > 0 ?
                        $"Вартість запчастин: {maintenance.PartsCost:C2}\n" :
                        "Запчастини не використовувались\n";

                    MessageBox.Show($"Деталі обслуговування #{maintenance.Id}\n\n" +
                                   $"Вартість робіт: {maintenance.LaborCost:C2}\n" +
                                   partsInfo +
                                   $"Загальна вартість: {maintenance.TotalCost:C2}\n" +
                                   $"Дата: {maintenance.Date:dd.MM.yyyy HH:mm}",
                                   "Деталі обслуговування",
                                   MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Виберіть запис обслуговування для перегляду деталей", "Інформація",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка перегляду деталей: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                SoundPlayerService.PlayErrorSound();
            }
        }

        private void RefreshMaintenance_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        // === ЗВІТИ ===
        private void RefreshReports_Click(object sender, RoutedEventArgs e)
        {
            RefreshReports();
        }

        private void RefreshReports()
        {
            try
            {
                var totalCars = _carService?.GetAll()?.Count ?? 0;
                var totalOrders = _orderService?.GetAll()?.Count ?? 0;
                var totalClients = _clientService?.GetAll()?.Count ?? 0;
                var totalParts = _inventoryService?.GetAll()?.Count ?? 0;
                var totalMaintenance = _maintenanceService?.GetAllMaintenanceRecords()?.Count ?? 0;
                var balance = _financeService?.GetBalance() ?? 0;

                ReportsBlock.Text = $"📊 ЗАГАЛЬНА СТАТИСТИКА:\n\n" +
                                   $"👥 Клієнтів: {totalClients}\n" +
                                   $"🚗 Автомобілів: {totalCars}\n" +
                                   $"📋 Замовлень: {totalOrders}\n" +
                                   $"🔧 Обслуговувань: {totalMaintenance}\n" +
                                   $"🔩 Запчастин на складі: {totalParts}\n" +
                                   $"💰 Баланс: {balance:C2}\n\n" +
                                   $"📅 Оновлено: {DateTime.Now:dd.MM.yyyy HH:mm}";

                SoundPlayerService.PlaySuccessSound();
            }
            catch (Exception ex)
            {
                ReportsBlock.Text = $"❌ Помилка оновлення звітів:\n{ex.Message}";
                SoundPlayerService.PlayErrorSound();
            }
        }
    }
}