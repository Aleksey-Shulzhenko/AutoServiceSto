using AutoServiceSto.Models;
using AutoServiceSto.Services;
using System;
using System.Windows;

namespace AutoServiceSto.Windows
{
    public partial class AddCarWindow : Window
    {
        private readonly CarService _carService;

        public AddCarWindow(CarService carService)
        {
            InitializeComponent();
            _carService = carService;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(BrandTextBox.Text) ||
                    string.IsNullOrWhiteSpace(ModelTextBox.Text) ||
                    string.IsNullOrWhiteSpace(LicensePlateTextBox.Text) ||
                    string.IsNullOrWhiteSpace(ClientIdTextBox.Text))
                {
                    MessageBox.Show("Заповніть обов'язкові поля: Марка, Модель, Номер, ID клієнта", "Помилка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!int.TryParse(YearTextBox.Text, out int year) || year < 1900 || year > 2030)
                {
                    MessageBox.Show("Введіть коректний рік (1900-2030)", "Помилка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!int.TryParse(ClientIdTextBox.Text, out int clientId) || clientId <= 0)
                {
                    MessageBox.Show("Введіть коректний ID клієнта", "Помилка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int mileage = 0;
                if (!string.IsNullOrWhiteSpace(MileageTextBox.Text) && !int.TryParse(MileageTextBox.Text, out mileage))
                {
                    MessageBox.Show("Введіть коректний пробіг", "Помилка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Добавляем автомобиль со всеми полями
                _carService.Add(
                    BrandTextBox.Text.Trim(),
                    ModelTextBox.Text.Trim(),
                    LicensePlateTextBox.Text.Trim(),
                    clientId,
                    year,
                    VINTextBox.Text.Trim(),
                    mileage
                );

                MessageBox.Show("Автомобіль успішно додано!", "Успіх",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка додавання автомобіля: {ex.Message}", "Помилка",
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