using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Database; // Contains your database connection and methods
using Models;   // Contains your model classes like Producto

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        // Constructor
        public Window3()
        {
            InitializeComponent();
            // Load data when the window is loaded
            this.Loaded += Window3_Loaded;
        }

        /// <summary>
        /// Event handler for when the window is loaded.
        /// Fills the DataGrid with all products asynchronously.
        /// </summary>
        private async void Window3_Loaded(object sender, RoutedEventArgs e)
        {
            await FillAllProductsAsync();
        }

        /// <summary>
        /// Asynchronous method to fill the DataGrid with all products.
        /// </summary>
        private async Task FillAllProductsAsync()
        {
            try
            {
                // Corrected the class name from ProductMagager to ProductManager
                List<Producto> productos = await ProductManager.GetProductsAsync();

                // Bind the product list to the DataGrid (dGProd_Stock)
                dGProd_Stock.ItemsSource = productos;
            }
            catch (Exception ex)
            {
                // Display an error message if there's an exception
                MessageBox.Show("Error al cargar los productos: " + ex.Message);
            }
        }

        /// <summary>
        /// Event handler for when the 'Check Stock' button is clicked.
        /// Triggers the check for products with zero stock.
        /// </summary>
        private async void btn_check_stock_Click(object sender, RoutedEventArgs e)
        {
            await CheckLowStockProductsAsync();
        }

        /// <summary>
        /// Asynchronous method to check for low stock products and update DataGrid and Label.
        /// </summary>
        private async Task CheckLowStockProductsAsync()
        {
            try
            {
                // Call the method in ProductManager to get products with zero stock
                List<Producto> productosSinStock = await Get_Low_Stock_Products.StockCeroAsync();

                // Bind the product list with zero stock to the DataGrid
                dGProd_Stock.ItemsSource = productosSinStock;

                // Update the label (lb_stock) based on whether there are products with zero stock
                if (productosSinStock.Count == 0)
                {
                    lb_stock.Content = "Ningún producto sin stock.";
                }
                else
                {
                    lb_stock.Content = $"{productosSinStock.Count} productos sin stock.";
                }
            }
            catch (Exception ex)
            {
                // Display the error message in the label
                lb_stock.Content = "Error: " + ex.Message;
            }
        }
    }
}

