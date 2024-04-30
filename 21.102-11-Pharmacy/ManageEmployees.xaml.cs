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
            // Проверка наличия всех необходимых данных
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtSurname.Text) ||
                string.IsNullOrEmpty(txtPositionId.Text) || string.IsNullOrEmpty(txtContactInfo.Text) ||
                string.IsNullOrEmpty(txtPassportSeries.Text) || string.IsNullOrEmpty(txtPassportNumber.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Проверка формата данных, например, для Position ID
            if (!int.TryParse(txtPositionId.Text, out _))
            {
                MessageBox.Show("Invalid Position ID. Please enter a valid integer.");
                return;
            }

            // Логика добавления нового сотрудника в базу данных
            employees newEmployee = new employees
            {   user_id= int.Parse(txtUserId.Text),
                name = txtName.Text,
                surname = txtSurname.Text,
                patronymic = txtPatronymic.Text,
                position_id = int.Parse(txtPositionId.Text),
                phone_number = txtContactInfo.Text,
                passport_series = txtPassportSeries.Text,
                passport_number = txtPassportNumber.Text,
            };

            using (var db = new Entities())
            {
                db.employees.Add(newEmployee);
                db.SaveChanges();
                LoadEmployeesData();
            }

            // Очистка полей после добавления
            txtName.Text = "";
            txtSurname.Text = "";
            txtPatronymic.Text = "";
            txtPositionId.Text = "";
            txtContactInfo.Text = "";
            txtPassportSeries.Text = "";
            txtPassportNumber.Text = "";
            txtUserId.Text = "";
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

                // Получаем обновленные данные из текстовых полей
                selectedEmployee.name = txtName.Text;
                selectedEmployee.surname = txtSurname.Text;
                selectedEmployee.patronymic = txtPatronymic.Text;
                selectedEmployee.position_id = int.Parse(txtPositionId.Text);
                selectedEmployee.phone_number = txtContactInfo.Text;
                selectedEmployee.passport_series = txtPassportSeries.Text;
                selectedEmployee.passport_number = txtPassportNumber.Text;

                using (var db = new Entities())
                {
                    // Обновляем данные о сотруднике в базе данных
                    db.Entry(selectedEmployee).State = EntityState.Modified;
                    db.SaveChanges();

                    // Перезагружаем данные в DataGrid
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
            string searchTerm = txtSearch.Text.Trim(); // Значение поискового запроса
            SearchEmployees(searchTerm);
        }
    }
}