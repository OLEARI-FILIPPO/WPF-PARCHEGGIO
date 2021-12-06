using System.Windows.Controls;

using WPF_DEFINITIVO.ViewModels;

namespace WPF_DEFINITIVO.Views
{
    public partial class StoricoPage : Page
    {
        public StoricoPage(StoricoViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
