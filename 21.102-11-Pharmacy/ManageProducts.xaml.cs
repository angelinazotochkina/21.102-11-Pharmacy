using _21._102_11_Pharmacy.Model;
using System;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace _21._102_11_Pharmacy
{
    public partial class ManageProducts : Window
    { 
        public ManageProducts()
        {
            InitializeComponent();
            LoadProductsData();
        }

        private void LoadProductsData()
        {
            using (var db = new Entities())
            {
                var pharmacyProducts = db.pharmacyproducts.ToList();
                PharmacyProductsDataGrid.ItemsSource = pharmacyProducts;
            }
        }
        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtCategory.Text) || string.IsNullOrEmpty(txtManufacturer.Text) || string.IsNullOrEmpty(txtPrice.Text) || string.IsNullOrEmpty(txtStatus.Text) || string.IsNullOrEmpty(txtQuantity.Text) || string.IsNullOrEmpty(txtDescription.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return false;
            }

            decimal price;
            if (!decimal.TryParse(txtPrice.Text, out price))
            {
                MessageBox.Show("Invalid Price. Please enter a valid decimal number.");
                return false;
            }

            int quantity;
            if (!int.TryParse(txtQuantity.Text, out quantity))
            {
                MessageBox.Show("Invalid Quantity. Please enter a valid integer.");
                return false;
            }

            return true;
        }

        private void UpdatePharmacyProduct_Click(object sender, RoutedEventArgs e)
        {
            if (PharmacyProductsDataGrid.SelectedItem != null)
            {
                pharmacyproducts selectedProduct = (pharmacyproducts)PharmacyProductsDataGrid.SelectedItem;

                // Заполнение полей формы данными выбранного продукта
                txtName.Text = selectedProduct.name;
                txtCategory.Text = selectedProduct.category;
                txtManufacturer.Text = selectedProduct.manufacturer;
                txtPrice.Text = selectedProduct.price.ToString();
                txtStatus.Text = selectedProduct.status;
                txtQuantity.Text = selectedProduct.stock_quantity.ToString();
                txtDescription.Text = selectedProduct.description;
                txtArticle.Text = selectedProduct.article_number;
            }
        }

        private void AddPharmacyProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            try
            {
                using (var db = new Entities())
                {
                    pharmacyproducts newProduct = new pharmacyproducts
                    {
                        name = txtName.Text,
                        category = txtCategory.Text,
                        manufacturer = txtManufacturer.Text,
                        price = Decimal.Parse(txtPrice.Text), 
                        status = txtStatus.Text,
                        stock_quantity = int.Parse(txtQuantity.Text),
                        description = txtDescription.Text,
                        article_number= txtArticle.Text
                    };

                    db.pharmacyproducts.Add(newProduct);
                    db.SaveChanges();
                    LoadProductsData(); 
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        MessageBox.Show($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }
            }
        }

        private void DeletePharmacyProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new Entities())
                {
                    if (PharmacyProductsDataGrid.SelectedItem != null)
                    {
                        pharmacyproducts selectedProduct = (pharmacyproducts)PharmacyProductsDataGrid.SelectedItem;
                        db.Entry(selectedProduct).State = EntityState.Deleted;
                        db.SaveChanges();
                        LoadProductsData(); 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при удалении продукта: {ex.Message}");
            }
        }

        private void SavePharmacyProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }


            using (var db = new Entities())
            {
                db.SaveChanges();
                LoadProductsData();
            }
        }

        private void SearchPharmacyProducts(string searchTerm)
        { 
            using (var db = new Entities())
            {
                var searchResults = db.pharmacyproducts.Where(product =>
                    product.name.Contains(searchTerm) ||
                    product.category.Contains(searchTerm) ||
                    product.article_number.Contains(searchTerm) ||
                    product.manufacturer.Contains(searchTerm)
                ).ToList();

                PharmacyProductsDataGrid.ItemsSource = searchResults;
            }
        }
     
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            SearchPharmacyProducts(searchTerm);
         
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
            txtCategory.Text = "";
            txtManufacturer.Text = "";
            txtPrice.Text = "";
            txtStatus.Text = "";
            txtQuantity.Text = "";
            txtDescription.Text = "";
            txtArticle.Text = "";
        }


    }
}