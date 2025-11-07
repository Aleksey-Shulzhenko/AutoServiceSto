using AutoServiceSto.Models;
using AutoServiceSto.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AutoServiceSto.Windows
{
    public partial class MaintenanceWindow : Window
    {
        private readonly CarService _carService;
        private readonly MaintenanceService _maintenanceService;
        private List<MaintenancePart> _usedParts = new List<MaintenancePart>();

        public MaintenanceWindow(CarService carService, MaintenanceService maintenanceService)
        {
            InitializeComponent();

            _carService = carService ?? throw new ArgumentNullException(nameof(carService));
            _maintenanceService = maintenanceService ?? throw new ArgumentNullException(nameof(maintenanceService));

            LoadData();
            UpdateCosts();
        }

        private void LoadData()
        {
            try
            {
                // Загружаем автомобили
                var cars = _carService?.GetAll();
                if (cars != null && cars.Any())
                {
                    CarComboBox.ItemsSource = cars.Select(c => new
                    {
                        Id = c.Id,
                        DisplayName = $"{c.Brand} {c.Model} ({c.LicensePlate})"
                    }).ToList();
                }
                else
                {
                    MessageBox.Show("Не знайдено жодного автомобіля", "Інформація",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Загружаем доступные запчасти
                var availableParts = _maintenanceService?.GetAvailableParts();
                if (availableParts != null && availableParts.Any())
                {
                    AvailablePartsComboBox.ItemsSource = availableParts;
                }
                else
                {
                    AvailablePartsComboBox.ItemsSource = new List<Part>();
                    MessageBox.Show("На складі немає доступних запчастин", "Інформація",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

                PartsGrid.ItemsSource = _usedParts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження даних: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddPart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AvailablePartsComboBox.SelectedItem is Part selectedPart)
                {
                    if (int.TryParse(QuantityTextBox.Text, out int quantity) && quantity > 0)
                    {
                        // Проверяем, достаточно ли запчастей на складе
                        if (selectedPart.QuantityInStock < quantity)
                        {
                            MessageBox.Show($"Недостатньо запчастин на складі. Доступно: {selectedPart.QuantityInStock}",
                                "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        // Проверяем, не добавлена ли уже эта запчасть
                        var existingPart = _usedParts.FirstOrDefault(p => p.PartId == selectedPart.Id);
                        if (existingPart != null)
                        {
                            existingPart.Quantity += quantity;
                        }
                        else
                        {
                            _usedParts.Add(new MaintenancePart
                            {
                                PartId = selectedPart.Id,
                                PartName = selectedPart.Name,
                                Quantity = quantity,
                                UnitPrice = selectedPart.Price
                            });
                        }

                        PartsGrid.Items.Refresh();
                        UpdateCosts();

                        // Сбрасываем выбор
                        AvailablePartsComboBox.SelectedIndex = -1;
                        QuantityTextBox.Text = "1";
                    }
                    else
                    {
                        MessageBox.Show("Введіть коректну кількість", "Помилка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Виберіть запчастину зі списку", "Помилка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка додавання запчастини: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemovePart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PartsGrid.SelectedItem is MaintenancePart part)
                {
                    _usedParts.Remove(part);
                    PartsGrid.Items.Refresh();
                    UpdateCosts();
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
            }
        }

        private void UpdateCosts()
        {
            try
            {
                decimal partsCost = _usedParts?.Sum(p => p.TotalCost) ?? 0;
                decimal laborCost = decimal.TryParse(LaborCostTextBox?.Text ?? "0", out decimal cost) ? cost : 0;
                decimal totalCost = partsCost + laborCost;

                // Проверяем, что элементы не null перед обновлением
                if (PartsCostText != null)
                    PartsCostText.Text = partsCost.ToString("C2");

                if (TotalCostText != null)
                    TotalCostText.Text = totalCost.ToString("C2");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка розрахунку вартості: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LaborCostTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCosts();
        }

        private void Perform_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CarComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Виберіть автомобіль", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
                {
                    MessageBox.Show("Введіть опис робіт", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!decimal.TryParse(LaborCostTextBox.Text, out decimal laborCost) || laborCost < 0)
                {
                    MessageBox.Show("Введіть коректну вартість робіт", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int carId = (int)CarComboBox.SelectedValue;

                _maintenanceService.PerformMaintenance(
                    carId,
                    DescriptionTextBox.Text,
                    _usedParts,
                    laborCost
                );

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка виконання обслуговування: {ex.Message}", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}