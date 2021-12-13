using MahApps.Metro.Controls;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebAPI_Definitivo;
using WebAPI_Definitivo.Models;
using WPF_DEFINITIVO.Helpers;
using WPF_DEFINITIVO.ViewModels;

namespace WPF_DEFINITIVO.Views
{
    /// <summary>
    /// Logica di interazione per UserPage.xaml
    /// </summary>
    public partial class UserPage : Page, INotifyPropertyChanged
    {
        UserViewModel logout;
        public UserPage(UserViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            logout = viewModel;

            username.Text = logout.Username;
            password.Text = logout.Password;

            
        }
        public event PropertyChangedEventHandler PropertyChanged;
        static HttpClient client = new HttpClient();

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private async void Button_Click(object sender, RoutedEventArgs e)//Bottone del logout
        {
           
            Users credenziali = new Users()
            {
                Username = logout.Username,
                Password = logout.Password
            };
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", logout.Token);

                var response = await client.PostAsJsonAsync("http://localhost:13636/api/v1/Logout", credenziali); //API controller name

                string result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    //dopo il logout lo stato torna a false e il resto diventa null;
                    NavigationLoginToLogout.isLoggedIn = false;
                    NavigationLoginToLogout.result = null;
                    NavigationLoginToLogout._user = null; 

                    //Tolgo menu tranne la schermata di login
                    for(int i = ShellViewModel.MenuItems.Count-1; i > 0; i--)
                    {
                        ShellViewModel.MenuItems.RemoveAt(i);
                    }

                    LoginPage user = new LoginPage(new LoginViewModel());
                    NavigationService.Navigate(user);
                }
                else
                {
                    MessageBox.Show("Error di logout","Error",MessageBoxButton.OK, MessageBoxImage.Error); //
                }
            }
            
        }

        private async void UserPageLoaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation d = new DoubleAnimation();
            d.From = 0;
            d.To = 1062;
            d.Duration = TimeSpan.FromSeconds(1);
            d.EasingFunction = new QuadraticEase();
            Out.BeginAnimation(WidthProperty, d);
            await logout.GetLast();
            lbLogin.Content = logout.lastLogin;
            lbLogout.Content = logout.lastLogout;
            //modifica.IsEnabled = false;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            username.Text = null;
           
            //modifica.IsEnabled = true;
        }

        private void TextBox_GotFocus_1(object sender, RoutedEventArgs e)
        {

            password.Text = null;
            //modifica.IsEnabled = true ;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {

            if(username.Text == "")
            {
                username.Text = "Username";
            }
            //modifica.IsEnabled = false;
        }

        private void TextBox_LostFocus_1(object sender, RoutedEventArgs e)
        {

            if(password.Text == "")
            {
                password.Text = "Password";
            }
            //modifica.IsEnabled= false ;
        }

        private async void modifica_Click(object sender, RoutedEventArgs e)
        {


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", logout.Token);
                string url = "http://localhost:13636/api/v1/ModifyUser/" + NavigationLoginToLogout._user.Username + "/" + NavigationLoginToLogout._user.Password + "/" + username.Text + "/" + password.Text;
                var response = await client.GetAsync(url); //API controller name

                string result = await response.Content.ReadAsStringAsync();
                if(username.Text != "" && password.Text != "")
                {
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Effetture il login con le nuove credenziali", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        //dopo il logout lo stato torna a false e il resto diventa null;
                        NavigationLoginToLogout.isLoggedIn = false;
                        NavigationLoginToLogout.result = null;
                        NavigationLoginToLogout._user = null;

                        //Tolgo menu tranne la schermata di login
                        for (int i = ShellViewModel.MenuItems.Count - 1; i > 0; i--)
                        {
                            ShellViewModel.MenuItems.RemoveAt(i);
                        }

                        LoginPage user = new LoginPage(new LoginViewModel());
                        NavigationService.Navigate(user);
                    }
                    else
                    {
                        string[] errore = result.Split("username");
                        if (errore.Length > 1)
                            System.Windows.MessageBox.Show("L'username attuale è la stessa del nuovo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        string[] errore2 = result.Split("password");
                        if (errore2.Length > 1)
                            System.Windows.MessageBox.Show("La password attuale è la stessa della nuova", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                    System.Windows.MessageBox.Show("Inserire tutti i campi", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
