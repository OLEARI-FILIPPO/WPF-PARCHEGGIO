using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation d = new DoubleAnimation();
            d.From = 0;
            d.To = grid.ActualHeight;
            d.Duration = TimeSpan.FromSeconds(0.4);
            d.EasingFunction = new QuadraticEase();
            grid.BeginAnimation(HeightProperty, d);
        }
    }
}
