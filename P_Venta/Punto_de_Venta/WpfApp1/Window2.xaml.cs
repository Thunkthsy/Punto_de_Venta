using Models;
using Database;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class Window2 : Window
    {
        // ObservableCollection to hold search results
        public ObservableCollection<Producto> Productos { get; set; }

        public Producto? SelectedProducto { get; private set; }

        public Window2()
        {
            InitializeComponent();
            Productos = new ObservableCollection<Producto>();
            DataContext = this; // Set DataContext for data binding
        }

        private async void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchText = txtSearch.Text.Trim();

                // Call the async method to search for products
                List<Models.Producto> productos = await Database.Search.ProductsAsync(searchText);

                // Clear existing items
                Productos.Clear();

                // Add new search results
                foreach (var producto in productos)
                {
                    Productos.Add(producto);
                }

                // Update message label
                if (productos.Count > 0)
                {
                    lblMessage.Visibility = Visibility.Collapsed;
                }
                else
                {
                    lblMessage.Content = "No se encontraron productos.";
                    lblMessage.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Content = "Error al buscar productos: " + ex.Message;
                lblMessage.Visibility = Visibility.Visible;
            }
        }


        private void dGProd_Search_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dGProd_Search.SelectedItem is Producto selectedProduct)
            {
                // Set the selected product
                SelectedProducto = selectedProduct;

                // Close Window2 and pass the product back to MainWindow
                this.DialogResult = true;
                this.Close();
            }
        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            // First, retrieve the clicked button's DataContext (which is a Producto)
            if ((sender as Button)?.DataContext is Producto selectedProduct)
            {
                // Set the selected product
                SelectedProducto = selectedProduct;

                // Close Window2 and pass the product back to MainWindow
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}

