using System;
using System.Windows;

namespace _21._102_11_Pharmacy
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Логика для обработки нажатия кнопки
        }


        private void ManageEmployees_Click(object sender, RoutedEventArgs e)
        {
          ManageEmployees employees = new ManageEmployees();
            employees.Show();
            Close();
        }

        private void GoToManagePharmacyProducts_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GoToManageUsers_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}