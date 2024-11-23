using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Database; // Provides database connection and methods to retrieve or manipulate data from the database.
using Models; // Defines the data models used in the application, Ticket and Product.

namespace WpfApp1
{
    
    // Implements INotifyPropertyChanged to enable property change notifications for data binding.
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        // Temporary list of Producto objects bound to the DataGrid (dgProducts).
        // Uses ObservableCollection for automatic UI updates on changes.
        public ObservableCollection<Producto> Productos { get; set; }

        // ObservableCollection to manage Ticket objects for database operations.
        // Each Ticket contains associated Producto objects.
        // Used for inserting and retrieving tickets and their products from the database.
        public ObservableCollection<Ticket> Tickets { get; set; } //Contains Products objects

        // ObservableCollection to maintain an updated list of ticket folio numbers extracted from the database.
        // These folio numbers are also used in the UI, in a ComboBox (CBoxTicket), to manage and display available ticket identifiers (folios).
        public ObservableCollection<int> TicketFolios { get; set; }

        // Private backing field to track the currently selected ticket folio.
        // This variable is used to control UI updates and prevent ticket duplication by comparing it with existing folios in the database.
        private int _Selected_Folio; // Variable to control UI changes

        public MainWindow() // Initialize the components of the MainWindow. 
        {
            InitializeComponent();

            Productos = new ObservableCollection<Producto>();

            Tickets = new ObservableCollection<Ticket>();
           
            TicketFolios = new ObservableCollection<int>();

            // Set the DataContext for data binding. This links the properties and collections of this class
            // to the UI, enabling dynamic updates and interactions between the UI and the code-behind.
            DataContext = this;

            // Call NewTicket to create a new ticket and set it as the selected ticket folio (Selected_Folio)
            NewTicket();

            // Populate the list of open ticket folios to display in the ComboBox.
            // This method fetches existing open tickets and populates TicketFolios accordingly.
            GetOpenTicketFolios();

        }

        // Implements INotifyPropertyChanged to notify the UI of changes, keeping it in sync with the selected folio.
        public int Selected_Folio
        {
            get => _Selected_Folio;
            set
            {

                if (_Selected_Folio != value)
                {
                    _Selected_Folio = value;
                    OnPropertyChanged(nameof(Selected_Folio)); // Notify the UI of changes to Folio.
                    UpdateFolio(); // Call UpdateFolio whenever Selected_Folio changes.
                    ChangeSelectedTicket();
                }
            }
        }

        //Method to reset dgProducts and retrieve products if ticket contains them on  database.
        private async void ChangeSelectedTicket()
        {
            // Retrieve the ticket by Selected_Folio.
            Ticket? ticket = await TicketManager.GetTicketByFolioAsync(Selected_Folio);

            if (ticket != null)
            {
                Productos.Clear(); // Clear all items from the Productos collection. 

                // Flags for tracking changes or issues
                bool priceChanged = false;
                bool insufficientStock = false;

                foreach (var ticketProduct in ticket.Productos)
                {
                    // Retrieve the current product details.
                    Producto? currentProduct = await ProductManager.GetProductByCodeAsync(ticketProduct.Codigo);

                    if (currentProduct != null)
                    {
                        // Skip products with stock = 0
                        if (currentProduct.UsaStock == 1 && currentProduct.Existencia == 0)
                        {
                            MessageBox.Show($"El producto con código {ticketProduct.Codigo} no tiene existencia y no será incluido.",
                                            "Producto sin stock", MessageBoxButton.OK, MessageBoxImage.Warning);
                            continue; // Skip this product
                        }

                        // Check for price change
                        if (ticketProduct.Precio != currentProduct.Precio)
                        {
                            priceChanged = true;
                            ticketProduct.Precio = currentProduct.Precio;
                        }

                        // Check for stock availability
                        if (currentProduct.UsaStock == 1 && ticketProduct.Cantidad > currentProduct.Existencia)
                        {
                            insufficientStock = true;
                            ticketProduct.Cantidad = currentProduct.Existencia; // Adjust the quantity to the available stock
                        }

                        // Update product details
                        ticketProduct.Nombre = currentProduct.Nombre;
                        ticketProduct.Descripcion = currentProduct.Descripcion;
                        ticketProduct.Existencia = currentProduct.Existencia;
                        ticketProduct.Medida = currentProduct.Medida;
                        ticketProduct.UsaStock = currentProduct.UsaStock;
                        ticketProduct.Departamento = currentProduct.Departamento;

                        // Add the updated product to the collection
                        Productos.Add(ticketProduct);
                    }
                    else
                    {
                        MessageBox.Show($"El producto con código {ticketProduct.Codigo} no existe en el sistema.",
                                        "Producto no encontrado", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

                // Update the total and folio label in the UI
                UpdateTotal();
                UpdateFolio();

                // Notify user of changes
                if (priceChanged)
                {
                    MessageBox.Show("Los precios de algunos productos han cambiado.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                if (insufficientStock)
                {
                    MessageBox.Show("Este ticket contenía productos con cantidades mayores a las del inventario registrado. Las cantidades se han ajustado al stock disponible.",
                                    "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                // Clear UI if no ticket is found
                Productos.Clear();
                UpdateTotal();
                UpdateFolio();
            }
        }

        // Asynchronously create a new ticket and retrieve its folio number.
        // Set this new ticket folio as the selected ticket folio.
        private async void NewTicket()
        {
            try
            {
                // Await the asynchronous task to complete and set Selected_Folio
                Selected_Folio = await TicketManager.CreateNewTicketAsync();
            }
            catch (Exception ex)
            {
                // Show an error message if there’s an issue creating the ticket
                MessageBox.Show($"Error en generar un nuevo ticket: {ex.Message}");
            }
            // Update the label with the current selected folio (the newly created ticket).
            UpdateFolio();
            // Refresh the list of open ticket folios to ensure it includes the new ticket.
            GetOpenTicketFolios();
        }

        // Clear existing items in TicketFolios to prepare for updated data.
        private async void GetOpenTicketFolios()
        {
            try
            {
                // Clear existing items in TicketFolios and add the new ones
                TicketFolios.Clear();

                // Retrieve a list of open ticket folio IDs asynchronously from the TicketManager.
                var openFolios = await TicketManager.GetOpenFoliosAsync();
                // Add each open folio ID to the TicketFolios collection.
                // This will automatically update any UI elements bound to TicketFolios.
                foreach (var folio in openFolios)
                {
                    TicketFolios.Add(folio);
                }
            }
            catch (Exception ex)
            {
                // Show an error message if there’s an issue retrieving the open ticket folios
                MessageBox.Show($"Error recuperando los folios de los tickets abiertos: {ex.Message}");
            }
        }


        // Event handler for the 'Search' button click
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
                    MessageBox.Show("No se seleccionó ningún producto.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        // Adds a product to the DataGrid, avoiding duplication and ensuring stock rules are followed.
        // Updates the total cost label.
        private void AddProductToDataGrid(Producto product)
        {
            // Validate that the product is not null
            if (product == null)
            {
                DisplayMessage("El producto es nulo y no se puede agregar.", "Error", MessageBoxImage.Error);
                return;
            }

            // Step 1: Verify if the product uses stock and if its stock is zero
            if (product.UsaStock == 1 && product.Existencia == 0)
            {
                DisplayMessage("El producto no se puede agregar porque su stock es cero.", "Error de Stock", MessageBoxImage.Error);
                return;
            }

            // Step 2: Check if the product already exists in the collection
            var existingProduct = Productos.FirstOrDefault(p => p.Codigo == product.Codigo);

            if (existingProduct != null)
            {
                // Step 3: Check if adding one more exceeds the available stock
                if (product.UsaStock == 1 && existingProduct.Cantidad + 1 > product.Existencia)
                {
                    DisplayMessage("El producto no se puede agregar porque excede el stock disponible.", "Error de Stock", MessageBoxImage.Error);
                    return;
                }

                // Step 4: Increment the quantity if stock is sufficient
                existingProduct.Cantidad += 1;
                DisplayMessage("Cantidad del producto aumentada.", "Información", MessageBoxImage.Information);
            }
            else
            {
                // Step 5: Add the new product to the collection
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

                Productos.Add(newProduct);
                DisplayMessage("Producto agregado.", "Información", MessageBoxImage.Information);
            }

            // Step 6: Update the total cost
            UpdateTotal();
        }


        // Displays a message in both the label and a MessageBox, with an optional icon.
        private void DisplayMessage(string labelMessage, string boxTitle, MessageBoxImage icon)
        {
            lblProductos.Content = labelMessage;
            lblProductos.Visibility = Visibility.Visible;

            // Show the MessageBox only for errors and critical warnings
            if (icon == MessageBoxImage.Error || icon == MessageBoxImage.Warning)
            {
                MessageBox.Show(labelMessage, boxTitle, MessageBoxButton.OK, icon);
            }
        }

        // Updates the total amount displayed in the UI by summing up the prices of all products.
        private void UpdateTotal()
        {
            decimal total = Productos.Sum(p => p.Precio * p.Cantidad);
            lblTotal.Content = $"Total: {total:N2}";
        }

        // Updates the label displaying the current selected ticket folio in the UI.
        private void UpdateFolio()
        {
            LblTicketFolio.Content = $"Ticket Actual: {Selected_Folio}";
        }

        // Handles the Enter button click event. Searches for a product by its code and adds it to the DataGrid if found.
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
                // If the input is not a valid integer, attempt to search by name
                try
                {
                    var products = await ProductManager.GetProductsByNameAsync(codeText);

                    if (products != null && products.Count > 0)
                    {
                        foreach (var product in products)
                        {
                            AddProductToDataGrid(product);
                        }
                        lblProductos.Content = $"{products.Count} producto(s) encontrado(s) con nombre similar.";
                        lblProductos.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        lblProductos.Content = "No se encontraron productos con un nombre similar.";
                        lblProductos.Visibility = Visibility.Visible;
                    }
                }
                catch (Exception ex)
                {
                    lblProductos.Content = "Error al buscar productos por nombre: " + ex.Message;
                    lblProductos.Visibility = Visibility.Visible;
                }
            }
        }

        // Event handler for the KeyDown event on txtCode, which listens for specific key presses.
        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnEnter_Click(sender, e);
            }
        }

        // Event handler for the Quitar button click, which removes a product from the Productos Datagrid.
        private void Quitar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Producto product)
            {
                Productos.Remove(product);
                // Update the total amount displayed in the UI after the product is removed.
                UpdateTotal();
            }
        }

        //Method to update Total when changes occur in dGProductos
        private void dGProductos_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString() == "Cantidad")
            {
                // Commit the edit
                if (sender is DataGrid dataGrid)
                {
                    dataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                    // Update total after editing quantity
                    UpdateTotal();
                }
            }
        }

        // Handles the click event for the "Inventario" button.
        // Opens a window (Window3) to check low-stock products.
        private void btnInventario_Click(object sender, RoutedEventArgs e)
        {
            // Open Window3
            Window3 form3 = new Window3();
            if (form3.ShowDialog() == true)
            {
                // Handle any logic here after Form3 is closed, if needed
            }
        }

        // Handles the payment process when the "Cobrar" button is clicked.
        // Validates products, processes payment, updates stock, and saves ticket data.
        private async void btn_Cobrar_Click(object sender, RoutedEventArgs e)
        {
            // Validate that there are products in the ticket
            if (!Productos.Any())
            {
                MessageBox.Show("No hay productos en el ticket.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            GetOpenTicketFolios();//Extract existing ticket folios in Dtabase

            // Calculate the total of the products
            decimal total = Productos.Sum(p => p.Precio * p.Cantidad);

            // Create an instance of Window4 and pass the total
            Window4 paymentWindow = new Window4();
            paymentWindow.TotalAmount = total;  // Assign the total

            // Show the payment window
            if (paymentWindow.ShowDialog() == true)
            {
                // Get the amount paid and calculate the change
                decimal paymentAmount = paymentWindow.PaymentAmount;
                decimal change = paymentAmount - total;

                // Show the payment summary
                MessageBox.Show($"Total: {total:C}\nPago: {paymentAmount:C}\nCambio: {change:C}", "Resumen de Pago", MessageBoxButton.OK, MessageBoxImage.Information);

                // Update product quantities in the database
                try
                {
                    await ProductManager.UpdateStockAsync(Productos.ToList());
                    MessageBox.Show("Cantidad de productos actualizada en la base de datos.", "Actualización exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar la cantidad de productos: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                // Create a Ticket object and set TicketFecha to the current date and time
                Ticket ticket = new Ticket
                {
                    Folio = Selected_Folio,
                    Estado = 2, // Closed
                    TotalTicket = total, // Total calculated earlier
                    TicketFecha = DateTime.Now, // Set to current local date and time
                    Productos = new ObservableCollection<Producto>(Productos) // Copy the products
                };

                // Save the ticket and products to the database
                try
                {
                    // Check if ticket is on database 
                    if (TicketFolios.Contains(Selected_Folio))
                    {
                        // Code to execute if Selected_Folio exists in TicketFolios 
                        Console.WriteLine($"Folio {Selected_Folio} exists in the collection.");
                        await TicketManager.UpdateTicketAsync(ticket);
                        MessageBox.Show("Ticket guardado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {   
                        // Code to execute if Selected_Folio does not exist in TicketFolios
                        await TicketManager.SaveTicketAsync(ticket);
                        
                        Console.WriteLine($"Folio {Selected_Folio} does not exist in the collection.");
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar el ticket: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                // Clear the DataGrid (product collection) after payment
                Productos.Clear(); // Clear the product list

                // Update the total to zero after clearing the DataGrid
                UpdateTotal(); // Method that updates the total label

                // Create a new ticket
                NewTicket();

                // Update the open ticket collection:
                // This ticket's status is set to 2 (closed) and should be removed from the ComboBox options.
                GetOpenTicketFolios();

            }
            else
            {

                MessageBox.Show("El pago no fue completado.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Event handler to remove the placeholder text from the text box when it gains focus.
        // </summary>
        private void RemoveText(object sender, EventArgs e)
        {
            // Check if the text in the text box matches the placeholder text
            if (txtCode.Text == "Introduce el código del Producto o Nombre")
            {
                // Clear the placeholder text
                txtCode.Text = "";

                // Change the text color to black to indicate the user is typing
                txtCode.Foreground = Brushes.Black;
            }
        }

        // Event handler to add placeholder text to the text box when it loses focus and is empty.
        private void AddText(object sender, EventArgs e)
        {
            // Check if the text box is empty or contains only whitespace
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                // Add the placeholder text back to the text box
                txtCode.Text = "Introduce el código del Producto o Nombre";

                // Change the text color to gray to indicate it is placeholder text
                txtCode.Foreground = Brushes.Gray;
            }
        }

        // Implement INotifyPropertyChanged interface to notify UI of property changes
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Handles the click event for the "Pendiente" button. 
        // Saves the current ticket in the database and sets its status ("Estado") to 1
        private async void Button_Pendiente_Click(object sender, RoutedEventArgs e)
        {
            // Validate that there are products in the ticket
            if (!Productos.Any())
            {
                MessageBox.Show("No hay productos en el ticket.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Calculate the total of the products
            decimal total = Productos.Sum(p => p.Precio * p.Cantidad);

            // Create a Ticket object
            Ticket ticket = new Ticket
            {
                Folio = Selected_Folio,
                Estado = 1, // Pending / Open
                TotalTicket = total, // total calculated earlier
                Productos = new ObservableCollection<Producto>(Productos) // Copy the products
            };

            // Save the ticket and products to the database
            try
            {
                // Check if ticket is on database 
                if (TicketFolios.Contains(Selected_Folio))
                {
                    // Code to execute if Selected_Folio exists in TicketFolios
                    Console.WriteLine($"Folio {Selected_Folio} exists in the collection.");
                    await TicketManager.UpdateTicketAsync(ticket);
                    MessageBox.Show("Ticket guardado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Create a new Ticket if Selected_Folio does not exist in TicketFolios
                    await TicketManager.SaveTicketAsync(ticket);
                    Console.WriteLine($"El folio {Selected_Folio} no existe en la colección.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el ticket: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Clear the DataGrid (product collection) after saving
            Productos.Clear(); // Clear the product list

            // Update the total to zero after clearing the DataGrid
            UpdateTotal(); // Method that updates the total label

            // Create a new ticket
            NewTicket();
        }

        // Event handler for the "Ventas" button click. Opens a window to check closed tickets by date. ("estado"=2)
        private void BtnVentas_Click(object sender, RoutedEventArgs e)
        {
            Window1 form1 = new Window1();
            form1.ShowDialog();
        }
    }
}

