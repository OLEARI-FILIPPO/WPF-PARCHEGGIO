using System.Windows.Controls;

using WPF_DEFINITIVO.ViewModels;

namespace WPF_DEFINITIVO.Views
{
    public partial class MainPage : Page
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Create_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateOwner c = new CreateOwner();

            c.ShowDialog(); //visualizzo form crea
        }
    }
}
