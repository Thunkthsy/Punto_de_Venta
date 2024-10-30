﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Database; // Contains  database connection and methods
using Models;   // Contains  model classes like Producto

namespace WpfApp1
{

    public partial class Window3 : Window
    {
        public Window3()
        {
            InitializeComponent();
            // Load data when the window is loaded
            this.Loaded += Window3_Loaded;
        }

        // When the window loads, fill the DataGrid with all products asynchronously
        private async void Window3_Loaded(object sender, RoutedEventArgs e)
        {
            await FillAllProductsAsync();
        }

        // Asynchronous method to fill the DataGrid with all products
        private async Task FillAllProductsAsync()
        {
            try
            {
                List<Producto> productos = await ProductManager.GetProductsAsync();

                // Bind the product list to the DataGrid
                dGProd_Stock.ItemsSource = productos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los productos: " + ex.Message);
            }
        }

        // This method is triggered when the user clicks btn_check_stock
        private async void btn_check_stock_Click(object sender, RoutedEventArgs e)
        {
            await CheckLowStockProductsAsync();
        }

        // Asynchronous method to check for low stock products and update DataGrid and Label
        private async Task CheckLowStockProductsAsync()
        {
            try
            {
                // Call the method in ProductManager to get products with zero stock
                List<Producto> productosSinStock = await ProductManager.Get_Low_Stock_Products.StockCeroAsync();

                // Bind the product list with zero stock to the DataGrid
                dGProd_Stock.ItemsSource = productosSinStock;

                // Update the label based on whether there are products with zero stock
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

