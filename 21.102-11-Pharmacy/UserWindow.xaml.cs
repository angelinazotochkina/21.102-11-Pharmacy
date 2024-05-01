using _21._102_11_Pharmacy.Model;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace _21._102_11_Pharmacy
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        List<pharmacyproducts> selectedProducts = new List<pharmacyproducts>();
        private int clientId;
        public UserWindow(int clientId)
        {
            InitializeComponent();
            this.clientId = clientId;
        }

        private void SearchProducts_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new Entities())
            {
                cmbCategory.ItemsSource = db.pharmacyproducts.Select(p => p.category).Distinct().ToList();
                cmbManufacturer.ItemsSource = db.pharmacyproducts.Select(p => p.manufacturer).Distinct().ToList();

                string searchKeyword = txtSearchProduct.Text;
                string selectedCategory = cmbCategory.SelectedItem as string;
                string selectedManufacturer = cmbManufacturer.SelectedItem as string;


                int minPrice, maxPrice;


                // Проверка и преобразование минимальной и максимальной цен
                if (!int.TryParse(txtMinPrice.Text, out minPrice))
                {
                    minPrice = 0; // По умолчанию 0
                }

                if (!int.TryParse(txtMaxPrice.Text, out maxPrice))
                {
                    maxPrice = int.MaxValue; // По умолчанию максимальное значение
                }

                // Фильтрация продуктов 
                var filteredProducts = db.pharmacyproducts
                    .Where(p => string.IsNullOrEmpty(searchKeyword) || p.name.Contains(searchKeyword))
                    .Where(p => selectedCategory == null || p.category == selectedCategory)
                    .Where(p => selectedManufacturer == null || p.manufacturer == selectedManufacturer)
                    .Where(p => p.price >= minPrice && p.price <= maxPrice)
                    .ToList();
                // Очистить результаты предыдущего поиска
                lstSearchResults.Items.Clear();

                // Добавить найденные продукты в список результатов
                foreach (var product in filteredProducts)
                {
                    string info = $"{product.name} - {product.price} - {product.manufacturer} - {product.category} - {product.status}";
                    lstSearchResults.Items.Add(info);
                }
               
            }
        }

      

        private void cmbManufacturer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ClearProducts_Click(object sender, RoutedEventArgs e)
        {
            txtSearchProduct.Text = "";
            txtMinPrice.Text = "от руб.";
            txtMaxPrice.Text = "до руб.";

            cmbCategory.SelectedItem = null;
            cmbManufacturer.SelectedItem = null;

            lstSearchResults.Items.Clear();
        }


        private List<int> selectedProductIds = new List<int>();

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new Entities())
            {
                foreach (var selectedItem in lstSearchResults.SelectedItems)
                {
                    string selectedItemInfo = selectedItem as string;
                    if (selectedItemInfo != null)
                    {
                        string[] selectedInfoParts = selectedItemInfo.Split(new string[] { " - " }, StringSplitOptions.None);
                        if (selectedInfoParts.Length >= 1)
                        {
                            string selectedProductName = selectedInfoParts[0];
                            // Поиск идентификатора товара по его названию
                            int productId = db.pharmacyproducts
                                            .Where(p => p.name == selectedProductName)
                                            .Select(p => p.product_id)
                                            .FirstOrDefault();

                            if (productId != 0)
                            {
                                selectedProductIds.Add(productId);
                                lstCart.Items.Add(selectedItemInfo); // Добавление выбранного товара в корзину
                            }
                        }
                    }
                }
            }
        }

        private void PlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new Entities())
            {
                try 
                {
                    // Создание нового заказа
                    orders newOrder = new orders
                    { 
                       client_id = clientId, 
                        status = "В обработке", 
                        delivery_date = DateTime.Now.AddDays(7), 
                        order_date = DateTime.Now,
                        order_code = GenerateUniqueOrderCode() 
                    };

                    db.orders.Add(newOrder);
                    db.SaveChanges();

                    // Добавление деталей заказа
                    foreach (int productId in selectedProductIds)
                    {
                        pharmacyproducts product = db.pharmacyproducts.Find(productId);

                        if (product != null)
                        {
                             orderdetails orderDetail = new orderdetails
                            {
                                order_id = newOrder.order_id,
                                product_id = productId,
                                quantity = 1 
                            };

                            db.orderdetails.Add(orderDetail);
                        }
                    }

                    db.SaveChanges();
                    // Очистка корзины после успешного заказа
                    lstCart.Items.Clear();
                    selectedProductIds.Clear();

                    MessageBox.Show("Заказ успешно создан");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка создания заказа: " + ex.Message);
                }
            }
        }

        private string GenerateUniqueOrderCode()
        {
           
            string orderCode = "ORD" + DateTime.Now.ToString("yyyyMMddHHmmss");
            using (var db = new Entities())
            {
                var existingOrder = db.orders.FirstOrDefault(o => o.order_code == orderCode);

                if (existingOrder != null)
                {
                    return GenerateUniqueOrderCode();
                }
            }

            return orderCode;
        }

        private void logOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            Close();
        }
    }
}
    

