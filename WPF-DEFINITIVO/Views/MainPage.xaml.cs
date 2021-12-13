using System;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using WPF_DEFINITIVO.Helpers;
using WPF_DEFINITIVO.Models;
using WPF_DEFINITIVO.ViewModels;

namespace WPF_DEFINITIVO.Views
{
    public partial class MainPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        MainViewModel main;
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;


            main = viewModel;
        }

   
        private async void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
         
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

            await main.GetParking();
            await main.getVehicle();
            await main.calculateRevenue();
            await main.AllParkingsRev();

            dispParkings.Text = main.nParking;
            vehicles.Text = main.nVehicle;
            rev.Text = main.totRevenue;

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

        private async void Elimina_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (gridIncassi.SelectedItem != null)
            {
                IncassiDisplay selezionato = (IncassiDisplay)gridIncassi.SelectedItem;

                decimal? somma = 0;
                foreach (var a in main.IncassiDisplay)
                {
                    if (a.NamePark != selezionato.NamePark)
                    {
                        somma += a.Revenue;
                    }
                }
                foreach (var a in main.IncassiDisplay)
                {
                    if(a.NamePark == selezionato.NamePark)
                    {
                        using (var client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);

                            string url = "http://localhost:13636/api/v1/DeletePark/" + selezionato.NamePark;
                            var response = await client.GetAsync(url);
                            //await response.Content.ReadAsStringAsync();
                            var list = await response.Content.ReadAsStringAsync();

                            if(response.IsSuccessStatusCode)
                            {
                                await main.AllParkingsRev();
                                rev.Text = somma.ToString();
                                System.Windows.MessageBox.Show("Parcheggio eliminato correttamente", "Infomation", MessageBoxButton.OK, MessageBoxImage.Information);
                                break;
                            }
                            else
                                System.Windows.MessageBox.Show("Selezionare un parcheggio per l'eliminazione", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                            
                        }

                    }
                }
            }
            else
                System.Windows.MessageBox.Show("Selezionare un parcheggio per l'eliminazione", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        }
    }


}
