using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Models
{
    public class Ticket : INotifyPropertyChanged
    {

        private int _folio;  // Unique identifier for a ticket
        private int _estado; // 1 = Open, 2 = Closed
        private decimal _totalTicket; // This value is set using the following calculation:
                                      // `decimal total = Productos.Sum(p => p.Precio * p.Cantidad);`
                                      // Derived from the product collection in `dgProducts` in the main window,
                                      // then saved as a separate value in the database (historical price).

        private DateTime _ticketFecha; // Property for ticket date
        private ObservableCollection<Producto> _productos = new ObservableCollection<Producto>();

        public Ticket()
        {
            _productos.CollectionChanged += Productos_CollectionChanged;
            _ticketFecha = DateTime.Now; // Default to current date when a ticket is created
        }

        public int Folio
        {
            get => _folio;
            set { _folio = value; OnPropertyChanged(nameof(Folio)); }
        }

        public int Estado
        {
            get => _estado;
            set { _estado = value; OnPropertyChanged(nameof(Estado)); }
        }

        public decimal TotalTicket
        {
            get => _totalTicket;
            set { _totalTicket = value; OnPropertyChanged(nameof(TotalTicket)); }
        }

        public DateTime TicketFecha
        {
            get => _ticketFecha;
            set { _ticketFecha = value; OnPropertyChanged(nameof(TicketFecha)); }
        }

        public ObservableCollection<Producto> Productos
        {
            get => _productos;
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));

                _productos.CollectionChanged -= Productos_CollectionChanged;
                _productos = value;
                _productos.CollectionChanged += Productos_CollectionChanged;

                OnPropertyChanged(nameof(Productos));
                OnPropertyChanged(nameof(TotalAmount));
            }
        }

        // Calculated property for the total amount paid (using historical prices)
        public decimal TotalAmount => _productos.Sum(p => p.Precio * p.Cantidad);

        private void Productos_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (Producto oldItem in e.OldItems)
                {
                    oldItem.PropertyChanged -= Producto_PropertyChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (Producto newItem in e.NewItems)
                {
                    newItem.PropertyChanged += Producto_PropertyChanged;
                }
            }

            OnPropertyChanged(nameof(TotalAmount));
        }

        private void Producto_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Producto.Precio) || e.PropertyName == nameof(Producto.Cantidad))
            {
                OnPropertyChanged(nameof(TotalAmount));
            }
        }

        // Implement INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
