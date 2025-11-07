using AutoServiceSto.Models;
using System;
using System.Windows;

namespace AutoServiceSto.Windows
{
    public partial class EditOrderWindow : Window
    {
        public Order EditedOrder { get; private set; }

        public EditOrderWindow(Order order)
        {
            InitializeComponent();
            EditedOrder = order;

            // Заполняем поля данными заказа
            DescriptionTextBox.Text = order.Description;
            CarIdTextBox.Text = order.CarId.ToString();
            CostTextBox.Text = order.Cost.ToString();
            DatePicker.SelectedDate = order.Date;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                MessageBox.Show("Введіть опис замовлення", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(CarIdTextBox.Text, out int carId) || carId <= 0)
            {
                MessageBox.Show("Введіть коректний ID автомобіля", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(CostTextBox.Text, out decimal cost) || cost < 0)
            {
                MessageBox.Show("Введіть коректну вартість", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (DatePicker.SelectedDate == null)
            {
                MessageBox.Show("Виберіть дату", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Обновляем данные заказа
            EditedOrder.Description = DescriptionTextBox.Text;
            EditedOrder.CarId = carId;
            EditedOrder.Cost = cost;
            EditedOrder.Date = DatePicker.SelectedDate.Value;

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}