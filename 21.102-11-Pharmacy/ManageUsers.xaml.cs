using _21._102_11_Pharmacy.Model;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace _21._102_11_Pharmacy
{
    public partial class ManageUsers : Window
    {
        public ManageUsers()
        {
            InitializeComponent();
            LoadUsersData();
        }

        private void LoadUsersData()
        {
            using (var db = new Entities())
            {
                var users = db.users.ToList();
                UsersDataGrid.ItemsSource = users;
            }
        }

        private void UpdateUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem != null)
            {
                users selectedUser = (users)UsersDataGrid.SelectedItem;

                 txtUsername.Text = selectedUser.login;
                 txtPassword.Text = selectedUser.password;
                 txtRoleId.Text = selectedUser.role_id.ToString();
            }
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrEmpty(txtRoleId.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (!int.TryParse(txtRoleId.Text, out _))
            {
                MessageBox.Show("Invalid Role ID. Please enter a valid integer.");
                return;
            }

            users newUser = new users
            {
                login = txtUsername.Text,
                password = txtPassword.Text,
                role_id = int.Parse(txtRoleId.Text),
            };

            using (var db = new Entities())
            {
                db.users.Add(newUser);
                db.SaveChanges();
                LoadUsersData();
            }

            txtUsername.Text = "";
            txtPassword.Text = "";
            txtRoleId.Text = "";
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem != null)
            {
                users selectedUser = (users)UsersDataGrid.SelectedItem;

                using (var db = new Entities())
                {
                    var userToDelete = db.users.FirstOrDefault(u => u.user_id == selectedUser.user_id);
                    if (userToDelete != null)
                    {
                        db.users.Remove(userToDelete);
                        db.SaveChanges();
                        LoadUsersData();
                    }
                }
            }
        }

        private void SaveUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem != null)
            {
                users selectedUser = (users)UsersDataGrid.SelectedItem;

                selectedUser.login = txtUsername.Text;
                selectedUser.password = txtPassword.Text;
                selectedUser.role_id = int.Parse(txtRoleId.Text);

                using (var db = new Entities())
                {
                    db.Entry(selectedUser).State = EntityState.Modified;
                    db.SaveChanges();
                    LoadUsersData();
                }

                MessageBox.Show("User updated successfully!");
            }
            else
            {
                MessageBox.Show("Please select a user to update.");
            }
        }

        private void SearchUsers(string searchTerm)
        {
            using (var db = new Entities())
            {
                var searchResults = db.users.Where(u =>
                    u.login.Contains(searchTerm) 
                    //u.role_id.ToInt32().Contains(searchTerm)
                ).ToList();

                UsersDataGrid.ItemsSource = searchResults;
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            SearchUsers(searchTerm);
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
             AdminWindow admin = new AdminWindow();
           admin.Show();
            Close();
        }
    }
}