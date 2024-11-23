using System.ComponentModel;

namespace Models
{
    public class Producto : INotifyPropertyChanged
    {
        private int _codigo; //codigo
        private string? _nombre; //nombre
        private string? _descripcion; //descripcion
        private decimal _precio; //precio
        private int _existencia; //existencia
        private string? _medida; //id_medidas
        private int _cantidad; //// Quantity is managed in dGProductos in the main window.
        private string? _departamento; //id_departamento
        private int _usaStock; //usa_stock

        public int Codigo
        {
            get => _codigo;
            set { _codigo = value; OnPropertyChanged(nameof(Codigo)); }
        }

        public required string? Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(nameof(Nombre)); }
        }

        public required string? Descripcion
        {
            get => _descripcion;
            set { _descripcion = value; OnPropertyChanged(nameof(Descripcion)); }
        }

        public decimal Precio
        {
            get => _precio;
            set { _precio = value; OnPropertyChanged(nameof(Precio)); }
        }

        public int Existencia
        {
            get => _existencia;
            set { _existencia = value; OnPropertyChanged(nameof(Existencia)); }
        }

        public required string? Medida
        {
            get => _medida;
            set { _medida = value; OnPropertyChanged(nameof(Medida)); }
        }

        public int Cantidad
        {
            get => _cantidad;
            set { _cantidad = value; OnPropertyChanged(nameof(Cantidad)); }
        }

        public required string? Departamento
        {
            get => _departamento;
            set { _departamento = value; OnPropertyChanged(nameof(Departamento)); }
        }

        public int UsaStock
        {
            get => _usaStock;
            set { _usaStock = value; OnPropertyChanged(nameof(UsaStock)); }
        }

        // Implement INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}

