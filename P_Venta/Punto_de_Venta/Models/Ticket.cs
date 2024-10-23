using Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

public class Ticket : INotifyPropertyChanged
{
    private int _folio;
    private ObservableCollection<Producto> _productos;
    // Add other properties as needed, like TotalAmount

    public int Folio
    {
        get => _folio;
        set { _folio = value; OnPropertyChanged(nameof(Folio)); }
    }

    public ObservableCollection<Producto> Productos
    {
        get => _productos ??= new ObservableCollection<Producto>();
        set { _productos = value; OnPropertyChanged(nameof(Productos)); }
    }

    // Implement INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
