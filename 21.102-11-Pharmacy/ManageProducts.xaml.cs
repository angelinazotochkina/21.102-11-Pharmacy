using _21._102_11_Pharmacy.Model;
using System;
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
            }
        }

        private void AddPharmacyProduct_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new Entities())
            {
                pharmacyproducts newProduct = new pharmacyproducts
                {
                    name = txtName.Text,
                    category = txtCategory.Text,
                    manufacturer = txtManufacturer.Text,
                    price = Convert.ToDecimal(txtPrice.Text),
                    status = txtStatus.Text,
                    stock_quantity = Convert.ToInt32(txtQuantity.Text),
                    description = txtDescription.Text
                };

                db.pharmacyproducts.Add(newProduct);
                db.SaveChanges();
                LoadProductsData(); // Обновление отображения данных
            }
        }

        private void DeletePharmacyProduct_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new Entities())
            {
                if (PharmacyProductsDataGrid.SelectedItem != null)
                {
                    pharmacyproducts selectedProduct = (pharmacyproducts)PharmacyProductsDataGrid.SelectedItem;
                    db.pharmacyproducts.Remove(selectedProduct);
                    db.SaveChanges();
                    LoadProductsData(); // Обновление отображения данных
                }
            }
        }

        private void SavePharmacyProduct_Click(object sender, RoutedEventArgs e)
        {
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
    }
}