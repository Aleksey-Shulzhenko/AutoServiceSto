using AutoServiceSto.Models;
using System;
using System.Windows;

namespace AutoServiceSto.Windows
{
    public partial class EditClientWindow : Window
    {
        public Client EditedClient { get; private set; }

        public EditClientWindow(Client client)
        {
            InitializeComponent();
            EditedClient = client;

            // Заполняем поля данными клиента
            FullNameTextBox.Text = client.FullName;
            PhoneTextBox.Text = client.Phone;
            EmailTextBox.Text = client.Email;
            AddressTextBox.Text = client.Address;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(PhoneTextBox.Text))
            {
                MessageBox.Show("Заповніть обов'язкові поля: ПІБ та Телефон", "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Обновляем данные клиента
            EditedClient.FullName = FullNameTextBox.Text;
            EditedClient.Phone = PhoneTextBox.Text;
            EditedClient.Email = EmailTextBox.Text;
            EditedClient.Address = AddressTextBox.Text;

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