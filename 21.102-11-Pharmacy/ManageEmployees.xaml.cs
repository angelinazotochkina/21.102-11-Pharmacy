using _21._102_11_Pharmacy.Model;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace _21._102_11_Pharmacy
{
    public partial class ManageEmployees : Window
    {
        public ManageEmployees()
        {
            InitializeComponent();
            LoadEmployeesData();
        }

        private void LoadEmployeesData()
        {
            using (var db = new Entities())
            {
                var employees = db.employees.ToList();
                EmployeesDataGrid.ItemsSource = employees;
            }
        }

        private void UpdateEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem != null)
            {
                employees selectedEmployee = (employees)EmployeesDataGrid.SelectedItem;

                txtName.Text = selectedEmployee.name;
                txtSurname.Text = selectedEmployee.surname;
                txtPatronymic.Text = selectedEmployee.patronymic;
                txtPositionId.Text = selectedEmployee.position_id.ToString();
                txtContactInfo.Text = selectedEmployee.phone_number;
                txtPassportSeries.Text = selectedEmployee.passport_series;
                txtPassportNumber.Text = selectedEmployee.passport_number;

            }
        }






        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtSurname.Text) ||
                string.IsNullOrEmpty(txtPositionId.Text) || string.IsNullOrEmpty(txtContactInfo.Text) ||
                string.IsNullOrEmpty(txtPassportSeries.Text) || string.IsNullOrEmpty(txtPassportNumber.Text))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            if (!int.TryParse(txtPositionId.Text, out int positionId))
            {
                MessageBox.Show("Неверный формат введния идентификатора должности");
                return;
            }
            if (txtPassportSeries.Text.Length > 4 || txtPassportNumber.Text.Length > 6)
            {
                MessageBox.Show("Серия папсорта должна содержать не более 4-х символов, а номер не более 6");
                return;
            }


            using (var db = new Entities())
            {
                // Проверка position_id
                var existingPositions = db.positions.Select(p => p.position_id).ToList();
                if (!existingPositions.Contains(positionId))
                {
                    MessageBox.Show("Specified Position ID does not exist. Please enter a valid one.");
                    return;
                }
                
            
            
                int userId = int.Parse(txtUserId.Text);
               
                 var existingUser = db.users.FirstOrDefault(u => u.user_id == userId);
                if (existingUser == null)
                {
                    MessageBox.Show("User with the specified User ID does not exist. Please enter a valid User ID.");
                    return;
                } 
                var existingUsers = db.employees.Select(emp => emp.user_id).ToList();
                if (existingUsers.Contains(userId))
                {
                    MessageBox.Show("Employee with the specified User ID already exists. Please choose a different User ID.");
                    return;
                }



                // Логика добавления нового сотрудника в базу данных
                employees newEmployee = new employees
                {
                    user_id = int.Parse(txtUserId.Text),
                    name = txtName.Text,
                    surname = txtSurname.Text,
                    patronymic = txtPatronymic.Text,
                    position_id = int.Parse(txtPositionId.Text),
                    phone_number = txtContactInfo.Text,
                    passport_series = txtPassportSeries.Text,
                    passport_number = txtPassportNumber.Text,
                    
                    
                    
                };

              db.employees.Add(newEmployee);
                    db.SaveChanges();
                    LoadEmployeesData();
            }
            
        }

        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem != null)
            {
                employees selectedEmployee = (employees)EmployeesDataGrid.SelectedItem;

                // Логика удаления выбранного сотрудника
                using (var db = new Entities())
                {
                    var employeeToDelete = db.employees.FirstOrDefault(emp => emp.employee_id == selectedEmployee.employee_id);
                    if (employeeToDelete != null)
                    {
                        db.employees.Remove(employeeToDelete);
                        db.SaveChanges();
                        LoadEmployeesData();
                    }
                }
            }
        }

        private void SaveEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem != null)
            {
                employees selectedEmployee = (employees)EmployeesDataGrid.SelectedItem;

               
                selectedEmployee.name = txtName.Text;
                selectedEmployee.surname = txtSurname.Text;
                selectedEmployee.patronymic = txtPatronymic.Text;
                selectedEmployee.position_id = int.Parse(txtPositionId.Text);
                selectedEmployee.phone_number = txtContactInfo.Text;
                selectedEmployee.passport_series = txtPassportSeries.Text;
                selectedEmployee.passport_number = txtPassportNumber.Text;

                using (var db = new Entities())
                {
                   
                    db.Entry(selectedEmployee).State = EntityState.Modified;
                    db.SaveChanges();

                    
                    LoadEmployeesData();
                }

                MessageBox.Show("Employee updated successfully!");
            }
            else
            {
                MessageBox.Show("Please select an employee to update.");
            }
        }


        private void SearchEmployees(string searchTerm)
        {
            using (var db = new Entities())
            {
                var searchResults = db.employees.Where(emp =>
                    emp.name.Contains(searchTerm) ||
                    emp.surname.Contains(searchTerm) ||
                    emp.phone_number.Contains(searchTerm) ||
                    emp.passport_series.Contains(searchTerm) ||
                    emp.passport_number.Contains(searchTerm)
                ).ToList();

                EmployeesDataGrid.ItemsSource = searchResults;
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim(); 
            SearchEmployees(searchTerm);
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow admin = new AdminWindow();
            admin.Show();
            Close();
        }

        private void ClearFields_Click(object sender, RoutedEventArgs e)
        {
      
            txtName.Text = "";
            txtSurname.Text = "";
            txtPatronymic.Text = "";
            txtPositionId.Text = "";
            txtContactInfo.Text = "";
            txtPassportSeries.Text = "";
            txtPassportNumber.Text = "";
            txtUserId.Text = "";
        }
    }
}