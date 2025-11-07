using AutoServiceSto.Models;
using System;
using System.Windows;

namespace AutoServiceSto.Windows
{
    public partial class EditPartWindow : Window
    {
        public Part EditedPart { get; private set; }

        public EditPartWindow(Part part)
        {
            InitializeComponent();
            EditedPart = part;

            // Заполняем поля данными запчасти
            NameTextBox.Text = part.Name;
            QuantityTextBox.Text = part.QuantityInStock.ToString();
            PriceTextBox.Text = part.Price.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Введіть назву запчастини", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Введіть коректну кількість", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Введіть коректну ціну", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Обновляем данные запчасти
            EditedPart.Name = NameTextBox.Text;
            EditedPart.QuantityInStock = quantity;
            EditedPart.Price = price;

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