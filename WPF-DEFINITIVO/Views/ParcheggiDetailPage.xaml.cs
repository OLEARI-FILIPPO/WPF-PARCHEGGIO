using System.Windows;
using System.Windows.Controls;

using WPF_DEFINITIVO.ViewModels;

namespace WPF_DEFINITIVO.Views
{
    public partial class ParcheggiDetailPage : Window
    {
        string parkingName;
        public ParcheggiDetailPage(string _parkingName)
        {
            InitializeComponent();
            parkingName = _parkingName;
            ParkingNameLabel.Text = parkingName;
            DataContext = this;

            // DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EntraWindow entraWindow = new EntraWindow(new EntraWindowViewModel());
            this.Close();
            entraWindow.ShowDialog();
        }
    }
}
