using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Lógica de interacción para Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        public decimal TotalAmount { get; set; }
        public decimal PaymentAmount { get; private set; }

        public Window4()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Display the total amount
            lblTotalAmount.Content = TotalAmount.ToString("C");
        }

        private void Pay_Click(object sender, RoutedEventArgs e)
        {
            // Validate the payment input
            if (decimal.TryParse(txtPaymentAmount.Text, out decimal payment))
            {
                if (payment >= TotalAmount)
                {
                    // Set the PaymentAmount and close the dialog with a positive result
                    PaymentAmount = payment;
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("El monto ingresado es menor que el total.", "Error de Pago", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Monto de pago inválido.", "Error de Entrada", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
