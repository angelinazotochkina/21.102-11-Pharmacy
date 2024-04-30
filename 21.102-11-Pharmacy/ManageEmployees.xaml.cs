using _21._102_11_Pharmacy.Model;
using System;
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
                // Логика обновления выбранного сотрудника
                // Вам нужно будет открыть окно/форму для редактирования данных и сохранения изменений в базу данных
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
            {
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
    }
}