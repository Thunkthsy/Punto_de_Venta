using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Database;
using Models;

namespace WpfApp1
{
    public partial class Window1 : Window, INotifyPropertyChanged
    {
        // ObservableCollection to hold the closed ticket folios
        public ObservableCollection<int> ClosedFolios { get; set; } = new ObservableCollection<int>();

        // ObservableCollection to hold the products for the selected ticket
        public ObservableCollection<Producto> TicketProducts { get; set; } = new ObservableCollection<Producto>();

        private int _selectedTicket;
        public int SelectedTicket
        {
            get => _selectedTicket;
            set
            {
                if (_selectedTicket != value)
                {
                    _selectedTicket = value;
                    OnPropertyChanged(nameof(SelectedTicket));
                    // Fetch and display ticket details when SelectedTicket changes
                    _ = LoadTicketDetailsAsync(_selectedTicket);
                }
            }
        }

        public Window1()
        {
            InitializeComponent();
            DataContext = this; // Set DataContext for data binding
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Method to fetch closed folios by the selected date
        private async void GetClosedFoliosButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if a date is selected
                if (!calendar.SelectedDate.HasValue)
                {
                    MessageBox.Show("Please select a date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DateTime selectedDate = calendar.SelectedDate.Value;

                // Retrieve closed folios by the selected date
                List<int> closedFolios = await TicketManager.GetClosedFoliosByDateAsync(selectedDate);

                // Clear and populate ClosedFolios ObservableCollection
                ClosedFolios.Clear();
                foreach (var folio in closedFolios)
                {
                    ClosedFolios.Add(folio);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving closed ticket folios: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Method to load ticket details by folio
        private async Task LoadTicketDetailsAsync(int ticketFolio)
        {
            try
            {
                // Check if a ticket folio is selected
                if (ticketFolio == 0)
                {
                    MessageBox.Show("Please select a ticket folio.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                TicketProducts.Clear();

                // Get ticket by folio
                Ticket? ticket = await TicketManager.GetTicketByFolioAsync(ticketFolio);
                if (ticket != null)
                {
                    // Add products to the observable collection, which is bound to the DataGrid
                    foreach (var product in ticket.Productos)
                    {
                        TicketProducts.Add(product);
                    }
                }
                else
                {
                    MessageBox.Show("Ticket not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading ticket details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ListBoxFolios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxFolios.SelectedItem is int selectedFolio)
            {
                SelectedTicket = selectedFolio; // Update the SelectedTicket property
            }
        }
    }
}


