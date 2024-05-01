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
                MessageBox.Show("Пожалуйста заполните все поля");
                return;
            }

            if (!int.TryParse(txtRoleId.Text, out int roleId) || !IsRoleIdValid(roleId))
            {
                MessageBox.Show("Недействительный идентификатор роли пользователя. Пожалуйста, введите существующий role_id.");
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
        private bool IsRoleIdValid(int roleId)
        {
            using (var db = new Entities())
            {
                var existingRoles = db.users.Select(u => u.role_id).Distinct().ToList();
                return existingRoles.Contains(roleId);
            }
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

                if (!int.TryParse(txtRoleId.Text, out int roleId) || !IsRoleIdValid(roleId))
                {
                    MessageBox.Show("Недействительный идентификатор роли пользователя. Пожалуйста, введите существующий role_id.");
                    return;
                }

                selectedUser.login = txtUsername.Text;
                selectedUser.password = txtPassword.Text;
                selectedUser.role_id = int.Parse(txtRoleId.Text);

                using (var db = new Entities())
                {
                    db.Entry(selectedUser).State = EntityState.Modified;
                    db.SaveChanges();
                    LoadUsersData();
                }

                MessageBox.Show("Успешно обновлено");
            }
            else
            {
                MessageBox.Show("Выберите пользователя для редактирования");
            }
        }

        private void SearchUsers(string searchTerm)
        {
            using (var db = new Entities())
            {
                int roleIdSearchTerm;
                bool isInteger = int.TryParse(searchTerm, out roleIdSearchTerm);

                var searchResults = db.users.Where(u =>
                    u.login.Contains(searchTerm) ||
                    (isInteger && u.role_id == roleIdSearchTerm)
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

        private void ClearFields_Click(object sender, RoutedEventArgs e)
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtRoleId.Text = "";
        }
    }
}