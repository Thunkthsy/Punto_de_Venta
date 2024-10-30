// MainWindow.xaml.cs
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Database;
using Models;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        // ObservableCollection to hold products
        public ObservableCollection<Producto> Productos { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            // DataContext is set to this for data binding
            DataContext = this;

            // Bind the DataGrid to the Productos collection in currentTicket
            //dGProductos.ItemsSource = currentTicket.Productos;

            // Set DataContext for data binding if necessary
            //this.DataContext = currentTicket;
        }

        // Event handler for the 'Search' button click
        private async void btnSearch_Click(object sender, RoutedEventArgs e)
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
                    MessageBox.Show("No se seleccionó ningún producto.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }


        private void AddProductToDataGrid(Producto product)
        {
            // Verify if the product uses stock and if its existence is zero
            if (product.UsaStock == 1 && product.Existencia == 0)
            {
                lblProductos.Content = "El producto no se puede agregar porque su stock es cero.";
                lblProductos.Visibility = Visibility.Visible;
                MessageBox.Show("El producto no se puede agregar porque su stock es cero.", "Error de Stock", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Find if the product already exists in the collection
            var existingProduct = Productos.FirstOrDefault(p => p.Codigo == product.Codigo);

            if (existingProduct != null)
            {
                // Verify if adding one more exceeds the available stock
                if (product.UsaStock == 1 && existingProduct.Cantidad + 1 > product.Existencia)
                {
                    lblProductos.Content = "No hay suficiente producto en stock";
                    lblProductos.Visibility = Visibility.Visible;
                    MessageBox.Show("El producto no se puede agregar porque excede el stock disponible.","Error de Stock", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Increment the quantity if the stock is sufficient
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
                    Cantidad = 1, // Set the initial quantity to 1
                    UsaStock = product.UsaStock,
                    Departamento = product.Departamento
                };

                // Add the new product to the collection
                Productos.Add(newProduct);
                lblProductos.Content = "Producto agregado.";
                lblProductos.Visibility = Visibility.Visible;
            }

            // Update the total cost
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
                    // Call the async method to get product by code using the Search class
                    Producto? product = await Search.ProductByCodeAsync(codigo);

                    if (product != null)
                    {
                        AddProductToDataGrid(product);
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

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnEnter_Click(sender, e);
            }
        }

        private void Quitar_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
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
                DataGrid? dataGrid = sender as DataGrid;
                dataGrid?.CommitEdit(DataGridEditingUnit.Row, true);

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

        private async void btn_Cobrar_Click(object sender, RoutedEventArgs e)
        {
            // Calcular el total de los productos
            decimal total = Productos.Sum(p => p.Precio * p.Cantidad);

            // Crear una instancia de Window4 y pasar el total
            Window4 paymentWindow = new Window4();
            paymentWindow.TotalAmount = total;  // Asignar el total

            // Mostrar la ventana de pago
            if (paymentWindow.ShowDialog() == true)
            {
                // Obtener el monto pagado y calcular el cambio
                decimal paymentAmount = paymentWindow.PaymentAmount;
                decimal change = paymentAmount - total;

                // Mostrar el resumen del pago
                MessageBox.Show($"Total: {total:C}\nPago: {paymentAmount:C}\nCambio: {change:C}", "Resumen de Pago", MessageBoxButton.OK, MessageBoxImage.Information);

                // Actualizar las cantidades de productos en la base de datos
                try
                {
                    await ProductManager.UpdateStockAsync(Productos.ToList());
                    MessageBox.Show("Cantidad de productos actualizada en la base de datos.", "Actualización exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar la cantidad de productos: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                // Limpiar el DataGrid (colección de productos) después del pago
                Productos.Clear(); // Limpiar la lista de productos

                // Actualizar el total a cero después de limpiar el DataGrid
                UpdateTotal(); // Método que ya actualiza la etiqueta de total
            }
            else
            {
                MessageBox.Show("El pago no fue completado.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RemoveText(object sender, EventArgs e)
        {
            if (txtCode.Text == "Introduce el código del Producto o Nombre")
            {
                txtCode.Text = "";
                txtCode.Foreground = Brushes.Black;
            }
        }

        private void AddText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                txtCode.Text = "Introduce el código del Producto o Nombre";
                txtCode.Foreground = Brushes.Gray;
            }
        }

        // Implement INotifyPropertyChanged interface to notify UI of property changes
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
