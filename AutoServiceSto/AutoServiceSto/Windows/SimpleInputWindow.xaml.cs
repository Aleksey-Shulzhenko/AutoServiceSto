using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AutoServiceSto.Windows
{
    public partial class SimpleInputWindow : Window
    {
        private readonly List<TextBox> _boxes = new();
        public List<string> Values { get; private set; } = new();

        public SimpleInputWindow(List<string> labels)
        {
            InitializeComponent();

            foreach (var label in labels)
            {
                var tb = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
                Panel.Children.Add(new TextBlock
                {
                    Text = label,
                    Margin = new Thickness(0, 5, 0, 2)
                });
                Panel.Children.Add(tb);
                _boxes.Add(tb);
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Values.Clear();
            foreach (var box in _boxes)
                Values.Add(box.Text);
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
