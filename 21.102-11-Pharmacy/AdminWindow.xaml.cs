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
            ManageProducts products = new ManageProducts();
            products.Show();
            Close();
        }

        private void GoToManageUsers_Click(object sender, RoutedEventArgs e)
        {
            ManageUsers users = new ManageUsers();
           users.Show();
            Close();
        }

        private void logOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            Close();
        }
    }
}