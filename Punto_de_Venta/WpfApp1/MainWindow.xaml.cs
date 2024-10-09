// MainWindow.xaml.cs
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Database;
using Models;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        // ObservableCollection to hold products
        public ObservableCollection<Producto> Productos { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Productos = new ObservableCollection<Producto>();
            DataContext = this; // Set DataContext for data binding
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            // Open Window2 as a dialog
            Window2 window2 = new Window2();
            if (window2.ShowDialog() == true)
            {
                // Retrieve the selected product
                Models.Producto? selectedProduct = window2.SelectedProducto;

                if (selectedProduct != null)
                {
                    // Add the product to DataGrid
                    AddProductToDataGrid(selectedProduct);
                }
                else
                {
                    MessageBox.Show("No product was selected.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }


        private void AddProductToDataGrid(Producto product)
        {
            // Check if product already exists
            var existingProduct = Productos.FirstOrDefault(p => p.Codigo == product.Codigo);

            if (existingProduct != null)
            {
                existingProduct.Cantidad += 1;
                lblProductos.Content = $"Product already exists. Increased quantity.";
                lblProductos.Visibility = Visibility.Visible;
            }
            else
            {
                // Clone the product to avoid modifying the original instance
                var newProduct = new Producto
                {
                    Codigo = product.Codigo,
                    Nombre = product.Nombre,
                    Descripcion = product.Descripcion,
                    Precio = product.Precio,
                    Existencia = product.Existencia,
                    Medida = product.Medida,
                    Cantidad = 1
                };

                Productos.Add(newProduct);
                lblProductos.Content = "Product added to a new row.";
                lblProductos.Visibility = Visibility.Visible;
            }

            UpdateTotal();
        }

        private void UpdateTotal()
        {
            decimal total = Productos.Sum(p => p.Precio * p.Cantidad);
            lblTotal.Content = $"Total: {total:N2}";
        }

        private async void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            string codeText = txtCode.Text.Trim();
            if (int.TryParse(codeText, out int codigo))
            {
                try
                {
                    // Call the async method to get product by code
                    Models.Producto? product = await Database.Search.ProductByCodeAsync(codigo);

                    if (product != null)
                    {
                        AddProductToDataGrid(product);
                        lblProductos.Content = "Producto agregado";
                        lblProductos.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        lblProductos.Content = "Producto no registrado";
                        lblProductos.Visibility = Visibility.Visible;
                    }
                }
                catch (Exception ex)
                {
                    lblProductos.Content = "Error al buscar el producto: " + ex.Message;
                    lblProductos.Visibility = Visibility.Visible;
                }
            }
            else
            {
                lblProductos.Content = "Código inválido";
                lblProductos.Visibility = Visibility.Visible;
            }

            // Clear the textbox and set focus for the next entry
            txtCode.Text = "";
            txtCode.Focus();
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnEnter_Click(sender, e);
            }
        }

        private void Quitar_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                var product = button.DataContext as Producto;
                if (product != null)
                {
                    Productos.Remove(product);
                    UpdateTotal();
                }
            }
        }

        private void dGProductos_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString() == "Cantidad")
            {
                // Commit the edit
                DataGrid dataGrid = sender as DataGrid;
                dataGrid.CommitEdit(DataGridEditingUnit.Row, true);

                // Update total after editing quantity
                UpdateTotal();
            }
        }

        // Implement other event handlers as needed
        private void btnVentas_Click(object sender, RoutedEventArgs e)
        {
            // Implement functionality for Ventas button
        }

        private void btnInventario_Click(object sender, RoutedEventArgs e)
        {
            // Open Form3(
            Window3 form3 = new Window3();
            if (form3.ShowDialog() == true)
            {
                // Handle any logic here after Form3 is closed, if needed
            }
        }

        private void btnConfiguracion_Click(object sender, RoutedEventArgs e)
        {
            // Implement functionality for Configuración button
        }

        private void btnCorte_Click(object sender, RoutedEventArgs e)
        {
            // Implement functionality for Corte button
        }
    }
}
