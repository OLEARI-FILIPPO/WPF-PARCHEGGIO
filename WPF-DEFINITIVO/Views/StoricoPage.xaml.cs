using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using WebAPI_Definitivo.Models;
using WPF_DEFINITIVO.Helpers;
using WPF_DEFINITIVO.ViewModels;

namespace WPF_DEFINITIVO.Views
{
    public partial class StoricoPage : Page
    {
        StoricoViewModel storico;
        public StoricoPage(StoricoViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            ConfigHelper.Instance.SetLang("it");
            DataContext = viewModel;
            storico = viewModel;
            StoricoCard2.Visibility = Visibility.Hidden;
            StoricoGrid2.RowBackground = (SolidColorBrush)new BrushConverter().ConvertFrom("#f29d9d");
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation d2 = new DoubleAnimation();
            d2.From = 0;
            d2.To = StoricoCard.ActualWidth;
            d2.Duration = TimeSpan.FromSeconds(1);
            d2.EasingFunction = new QuadraticEase();
            StoricoCard.BeginAnimation(WidthProperty, d2);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation d = new DoubleAnimation();
            d.From = 0;
            d.To = operations.ActualWidth;
            d.Duration = TimeSpan.FromSeconds(1);
            d.EasingFunction = new QuadraticEase();
            operations.BeginAnimation(WidthProperty, d);

            dataHistory.SelectedDate = DateTime.Today;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //Salvo la data cosi la posso usare nel viewModel

            string[] dataDivisa = dataHistory.ToString().Split("/");


            HistoryHelper.giorno = Convert.ToInt32(dataDivisa[0]);
            HistoryHelper.mese = Convert.ToInt32(dataDivisa[1]);
            HistoryHelper.anno = Convert.ToInt32(dataDivisa[2].Substring(0, 4));

            //CHIAMA LA FUNZIONE DEL VIEW MODEL
            var sorico = (StoricoViewModel)DataContext;
            await storico.Refresh();

            if (HistoryHelper.check == false)
                StoricoCard2.Visibility = Visibility.Visible;
            else
                StoricoCard2.Visibility = Visibility.Hidden;

        }
    }
}
