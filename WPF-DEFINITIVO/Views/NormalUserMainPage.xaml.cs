using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_DEFINITIVO.Views
{
    /// <summary>
    /// Logica di interazione per NormalUserMainPage.xaml
    /// </summary>
    public partial class NormalUserMainPage : Page
    {
        public NormalUserMainPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void UsciteLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            gridUscite.Visibility = Visibility.Visible;
            Disponibili.Visibility = Visibility.Hidden;
            gridVeicoli.Visibility = Visibility.Hidden;
        }

        private void Card_MouseDoubleClick(object sender, MouseButtonEventArgs e) //parcheggi disponibili
        {
            gridUscite.Visibility = Visibility.Hidden;
            Disponibili.Visibility = Visibility.Visible;
            gridVeicoli.Visibility = Visibility.Hidden;
        }

        private void Card_MouseDoubleClick_1(object sender, MouseButtonEventArgs e) //veicoli disponibili
        {
            gridUscite.Visibility = Visibility.Hidden;
            Disponibili.Visibility = Visibility.Hidden;
            gridVeicoli.Visibility = Visibility.Visible;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            gridUscite.Visibility = Visibility.Visible;
            Disponibili.Visibility = System.Windows.Visibility.Hidden;
            gridVeicoli.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
