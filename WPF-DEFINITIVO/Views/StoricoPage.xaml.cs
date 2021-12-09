using HandyControl.Tools;
using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;
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

        private void DataGrid_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
       

            DoubleAnimation d2 = new DoubleAnimation();
            d2.From = 0;
            d2.To = StoricoCard.ActualWidth;
            d2.Duration = TimeSpan.FromSeconds(1);
            d2.EasingFunction = new QuadraticEase();
            StoricoCard.BeginAnimation(WidthProperty, d2);
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            DoubleAnimation d = new DoubleAnimation();
            d.From = 0;
            d.To = operations.ActualWidth;
            d.Duration = TimeSpan.FromSeconds(1);
            d.EasingFunction = new QuadraticEase();
            operations.BeginAnimation(WidthProperty, d);
        }
    }
}
