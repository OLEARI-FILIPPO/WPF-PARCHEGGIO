using System.Windows;
using System.Windows.Controls;

using WPF_DEFINITIVO.ViewModels;

namespace WPF_DEFINITIVO.Views
{
    public partial class ParcheggiDetailPage : Window
    {
        public ParcheggiDetailPage()
        {
            InitializeComponent();
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
