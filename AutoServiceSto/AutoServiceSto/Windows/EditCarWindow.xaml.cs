using AutoServiceSto.Models;
using System;
using System.Windows;

namespace AutoServiceSto.Windows
{
    public partial class EditCarWindow : Window
    {
        public Car EditedCar { get; private set; }

        public EditCarWindow(Car car)
        {
            InitializeComponent();

            // Создаем новый объект чтобы не менять оригинальный до сохранения
            EditedCar = new Car
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                LicensePlate = car.LicensePlate,
                VIN = car.VIN,
                Mileage = car.Mileage,
                ClientId = car.ClientId
            };

            // Заполняем поля данными автомобиля
            BrandTextBox.Text = EditedCar.Brand;
            ModelTextBox.Text = EditedCar.Model;
            YearTextBox.Text = EditedCar.Year.ToString();
            LicensePlateTextBox.Text = EditedCar.LicensePlate;
            VINTextBox.Text = EditedCar.VIN;
            MileageTextBox.Text = EditedCar.Mileage.ToString();
            ClientIdTextBox.Text = EditedCar.ClientId.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
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

                // Обновляем данные автомобиля
                EditedCar.Brand = BrandTextBox.Text.Trim();
                EditedCar.Model = ModelTextBox.Text.Trim();
                EditedCar.Year = year;
                EditedCar.LicensePlate = LicensePlateTextBox.Text.Trim();
                EditedCar.VIN = VINTextBox.Text.Trim();
                EditedCar.Mileage = mileage;
                EditedCar.ClientId = clientId;

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка збереження: {ex.Message}", "Помилка",
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