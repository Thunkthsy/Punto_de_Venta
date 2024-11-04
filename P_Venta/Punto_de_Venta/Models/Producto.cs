using System.ComponentModel;

namespace Models
{
    public class Producto : INotifyPropertyChanged
    {
        private int _codigo;
        private string? _nombre;
        private string? _descripcion;
        private decimal _precio;
        private int _existencia;
        private string? _medida;
        private int _cantidad;
        private string? _departamento;
        private int _usaStock;

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

        // Property for Cantidad
        public int Cantidad
        {
            get => _cantidad;
            set { _cantidad = value; OnPropertyChanged(nameof(Cantidad)); }
        }

        // Property for Departamento
        public string? Departamento
        {
            get => _departamento;
            set { _departamento = value; OnPropertyChanged(nameof(Departamento)); }
        }

        // Property for UsaStock
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

