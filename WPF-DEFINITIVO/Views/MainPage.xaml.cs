using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using WPF_DEFINITIVO.Helpers;
using WPF_DEFINITIVO.ViewModels;

namespace WPF_DEFINITIVO.Views
{
    public partial class MainPage : Page
    {
        MainViewModel main;
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;


            main = viewModel;
        }

   
        private async void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
           await main.GetParking();
           await main.getVehicle();
           await main.calculateRevenue();
            await main.AllParkingsRev();
            dispParkings.Text = main.nParking;
            vehicles.Text = main.nVehicle;
            rev.Text = main.totRevenue;
            //disp.ItemsSource = main.Source;
            if (NavigationLoginToLogout.UserPriviledge == 2)
            {
                NormalUserMainPage page = new NormalUserMainPage(); 

                NavigationService.Navigate(page);
            }
            else
            {
                DoubleAnimation d = new DoubleAnimation();
                d.From = 0;
                d.To = cardPanel.ActualHeight;
                d.Duration = TimeSpan.FromSeconds(0.4);
                d.EasingFunction = new QuadraticEase();
                cardPanel.BeginAnimation(HeightProperty, d);

                DoubleAnimation fadeAnimation = new DoubleAnimation();
                fadeAnimation.Duration = TimeSpan.FromSeconds(1.0d);
                fadeAnimation.EasingFunction = new QuadraticEase();
                fadeAnimation.From = 0.0d;
                fadeAnimation.To = 1.0d;
                gridPanel.BeginAnimation(Grid.OpacityProperty, fadeAnimation);


                Disponibili.Visibility = System.Windows.Visibility.Hidden;
                Utenti.Visibility = System.Windows.Visibility.Hidden;
            }

            

        }

        private void Incasso_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            Disponibili.Visibility = System.Windows.Visibility.Hidden;
            Utenti.Visibility = System.Windows.Visibility.Hidden;
            Incassi.Visibility = System.Windows.Visibility.Visible;
        }

        private void Disponibili_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            Disponibili.Visibility = System.Windows.Visibility.Visible;
            Utenti.Visibility = System.Windows.Visibility.Hidden;
            Incassi.Visibility = System.Windows.Visibility.Hidden;
        }

        private void NonDisponibili_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            Disponibili.Visibility = System.Windows.Visibility.Hidden;
            Utenti.Visibility = System.Windows.Visibility.Visible;
            Incassi.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           AddVehicle s = new AddVehicle();
            s.ShowDialog();
        }
    }
}
