using HandyControl.Tools;
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
            ConfigHelper.Instance.SetLang("it");
           // StoricoCard.BorderBrush.Opacity = 0;
            DataContext = viewModel;
        }
    }
}
