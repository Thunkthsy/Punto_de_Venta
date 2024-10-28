using System;
using System.Collections.ObjectModel;
using System.ComponentModel; 
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Database; 
using Models;   
using MySql.Data.MySqlClient;

namespace WpfApp1
{
    // MainWindow class inherits from Window and implements INotifyPropertyChanged for data binding
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        // Collection of products
        private ObservableCollection<Producto> _productos = new ObservableCollection<Producto>();
        public ObservableCollection<Producto> Productos
        {
            get => _productos;
            set
            {
                if (_productos != value)
                {
                    _productos = value;
                    OnPropertyChanged(nameof(Productos));
                }
            }
        }

        // Constructor
        public MainWindow()
        {
            InitializeComponent();
            Productos = new ObservableCollection<Producto>();
            DataContext = this;

            // Bind the DataGrid to the Productos collection
            dGProductos.ItemsSource = Productos;
        }

        // Event handler for the 'Search' button click
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            // Open Window2 as a dialog to select a product
            Window2 window2 = new Window2();
            if (window2.ShowDialog() == true)
            {
                // Retrieve the selected product
                Producto? selectedProduct = window2.SelectedProducto;

                if (selectedProduct != null)
                {
                    // Add the product to the collection
                    AddProduct(selectedProduct);
                }
                else
                {
                    MessageBox.Show("No product was selected.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        // Method to add a product to the collection
        private void AddProduct(Producto product)
        {
            // Check if the product uses stock tracking (UsaStock is 1) and if the stock is zero
            if (product.UsaStock == 1 && product.Existencia == 0)
            {
                // Display an error message if the product's stock is zero
                lblProductos.Content = "El producto no se puede agregar porque su stock es cero.";
                lblProductos.Visibility = Visibility.Visible;
                MessageBox.Show("El producto no se puede agregar porque su stock es cero.", "Error de Stock", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Check if the product already exists in the collection
            var existingProduct = Productos.FirstOrDefault(p => p.Codigo == product.Codigo);

            if (existingProduct != null)
            {
                // Check if adding one more would exceed the available stock
                if (product.UsaStock == 1 && existingProduct.Cantidad + 1 > product.Existencia)
                {
                    lblProductos.Content = "No hay suficiente producto en stock";
                    lblProductos.Visibility = Visibility.Visible;
                    MessageBox.Show("El producto no se puede agregar porque excede el stock disponible.", "Error de Stock", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Increase the quantity if the stock is sufficient
                existingProduct.Cantidad += 1;
                lblProductos.Content = "Cantidad del producto aumentada";
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
                    Cantidad = 1,           // Set initial quantity to 1
                    UsaStock = product.UsaStock,
                    Departamento = product.Departamento
                };

                // Add the new product to the collection
                Productos.Add(newProduct);
                lblProductos.Content = "Producto agregado.";
                lblProductos.Visibility = Visibility.Visible;
            }

            // Update the total label
            lblTotal.Content = $"Total: {TotalAmount:N2}";
        }

        // Property to calculate the total amount
        public decimal TotalAmount
        {
            get
            {
                return Productos.Sum(p => p.Precio * p.Cantidad);
            }
        }

        // Event handler for 'Enter' button click (searching product by code)
        private async void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            string codeText = txtCode.Text.Trim();
            if (int.TryParse(codeText, out int codigo))
            {
                try
                {
                    // Call the async method to get product by code
                    Producto? product = await Database.Search.ProductByCodeAsync(codigo);

                    if (product != null)
                    {
                        AddProduct(product);
                        lblProductos.Content = "Producto agregado";
                        lblProductos.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        lblProductos.Content = "Producto no registrado en la base de datos";
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

        // Event handler for when the 'Enter' key is pressed in txtCode textbox
        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnEnter_Click(sender, e);
            }
        }

        // Event handler for 'Remove' button click in the product DataGrid
        private void Quitar_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            if (button != null)
            {
                var product = button.DataContext as Producto;
                if (product != null)
                {
                    Productos.Remove(product);
                    // Update the total label
                    lblTotal.Content = $"Total: {TotalAmount:N2}";
                }
            }
        }

        // Event handler for when a cell edit is ending in the DataGrid
        private void dGProductos_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString() == "Cantidad")
            {
                // Commit the edit
                DataGrid? dataGrid = sender as DataGrid;
                dataGrid?.CommitEdit(DataGridEditingUnit.Row, true);

                // Update total after editing quantity
                lblTotal.Content = $"Total: {TotalAmount:N2}";
            }
        }

        // Event handler for 'Cobrar' button click
        private async void btn_Cobrar_Click(object sender, RoutedEventArgs e)
        {
            decimal total = TotalAmount;

            // Create an instance of Window4 (payment window) and pass the total
            Window4 paymentWindow = new Window4
            {
                TotalAmount = total  // Assign the total
            };

            // Show the payment window
            if (paymentWindow.ShowDialog() == true)
            {
                // Get the amount paid and calculate the change
                decimal paymentAmount = paymentWindow.PaymentAmount;
                decimal change = paymentAmount - total;

                // Show the payment summary
                MessageBox.Show($"Total: {total:C}\nPago: {paymentAmount:C}\nCambio: {change:C}", "Resumen de Pago", MessageBoxButton.OK, MessageBoxImage.Information);

                // Update the stock quantities of products in the database
                try
                {
                    await Database.ProductManager.UpdateStockAsync(Productos.ToList());
                    MessageBox.Show("Cantidad de productos actualizada en la base de datos.", "Actualización exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar la cantidad de productos: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                // Clear the products and reset total
                Productos.Clear();
                lblTotal.Content = $"Total: {TotalAmount:N2}";
                lblProductos.Content = "Venta completada.";
                lblProductos.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("El pago no fue completado.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Event handler for 'Ventas' button click
        private void btnVentas_Click(object sender, RoutedEventArgs e)
        {
            // Implement functionality for Ventas button
        }

        // Event handler for 'Inventario' button click
        private void btnInventario_Click(object sender, RoutedEventArgs e)
        {
            // Open Window3 (inventory management)
            Window3 form3 = new Window3();
            if (form3.ShowDialog() == true)
            {
                // Handle any logic here after Window3 is closed, if needed
            }
        }

        // Event handler for 'Configuración' button click
        private void btnConfiguracion_Click(object sender, RoutedEventArgs e)
        {
            // Implement functionality for Configuración button
        }

        // Event handler for 'Corte' button click
        private void btnCorte_Click(object sender, RoutedEventArgs e)
        {
            // Implement functionality for Corte button
        }

        // Event handler to remove placeholder text from txtCode when it gets focus
        private void RemoveText(object sender, EventArgs e)
        {
            if (txtCode.Text == "Introduce el código del Producto o Nombre")
            {
                txtCode.Text = "";
                txtCode.Foreground = Brushes.Black;
            }
        }

        // Event handler to add placeholder text to txtCode when it loses focus
        private void AddText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                txtCode.Text = "Introduce el código del Producto o Nombre";
                txtCode.Foreground = Brushes.Gray;
            }
        }

        // Implement INotifyPropertyChanged interface to notify UI of property changes
        public event PropertyChangedEventHandler? PropertyChanged;

        // Method to raise the PropertyChanged event
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

