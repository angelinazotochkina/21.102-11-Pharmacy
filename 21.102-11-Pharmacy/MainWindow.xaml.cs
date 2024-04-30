using _21._102_11_Pharmacy.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _21._102_11_Pharmacy
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            string username = txtLogin.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblMessage.Content = "Please enter both username and password.";
                return;
            }


            using (var db = new Entities())
            {
                var user = db.users.FirstOrDefault(u => u.login == username && u.password == password);


                if (user != null)
                { 
                    var client = db.clients.FirstOrDefault(c => c.user_id == user.user_id);

                    if (client != null)
                    {
                        int currentClientId = client.client_id;

                        switch (user.role_id)
                        {
                            case 1: // User role
                                UserWindow userWindow = new UserWindow(client.client_id);
                                userWindow.Show();
                                Close();

                                break;
                            case 2: // Employee role
                                EmployeeWindow employeeWindow = new EmployeeWindow();
                                employeeWindow.Show();
                                Close();
                                break;
                            case 3: // Admin role
                                AdminWindow adminWindow = new AdminWindow();
                                adminWindow.Show();
                                Close();
                                break;
                            default:
                                lblMessage.Content = "Invalid username or password. Please try again.";
                                break;
                        }
                    }
                    else
                    {
                        lblMessage.Content = "Invalid username or password. Please try again.";
                    }
                }
            }

           
        } 
        public void btnSkip_Click(object sender, RoutedEventArgs e)
            {
              //  UserWindow userWindow = new UserWindow();
             //   userWindow.Show();
              //  Close();
            }
    }
}
