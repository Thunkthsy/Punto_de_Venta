using Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Models
{
    public class Ticket : INotifyPropertyChanged
    {
        private int _idTicket;
        private int _folio;
        private int _estado;
        private decimal _totalTicket;
        private ObservableCollection<Producto> _productos = new ObservableCollection<Producto>();

        public Ticket()
        {
            _productos.CollectionChanged += Productos_CollectionChanged;
        }

        public int IdTicket
        {
            get => _idTicket;
            set
            {
                _idTicket = value;
                OnPropertyChanged(nameof(IdTicket));
            }
        }

        public int Folio
        {
            get => _folio;
            set
            {
                _folio = value;
                OnPropertyChanged(nameof(Folio));
            }
        }

        public int Estado
        {
            get => _estado;
            set
            {
                _estado = value;
                OnPropertyChanged(nameof(Estado));
            }
        }

        public decimal TotalTicket
        {
            get => _totalTicket;
            set
            {
                _totalTicket = value;
                OnPropertyChanged(nameof(TotalTicket));
            }
        }

        public ObservableCollection<Producto> Productos
        {
            get => _productos;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _productos.CollectionChanged -= Productos_CollectionChanged;

                _productos = value;
                _productos.CollectionChanged += Productos_CollectionChanged;
                OnPropertyChanged(nameof(Productos));
                OnPropertyChanged(nameof(TotalAmount));
            }
        }

        public decimal TotalAmount
        {
            get => _productos.Sum(p => p.Precio * p.Cantidad);
        }

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

        // Implementación de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class TicketFolio : IEquatable<TicketFolio>
    {
        public int Folio { get; set; }
        public string TicketDisplay => $"Ticket [{Folio}]";

        public bool Equals(TicketFolio? other)
        {
            if (other == null) return false;
            return this.Folio == other.Folio;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as TicketFolio);
        }

        public override int GetHashCode()
        {
            return Folio.GetHashCode();
        }
    }
}



