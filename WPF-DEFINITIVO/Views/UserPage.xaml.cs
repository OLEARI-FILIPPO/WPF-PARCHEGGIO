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
        }
        public event PropertyChangedEventHandler PropertyChanged;
        static HttpClient client = new HttpClient();

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
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

                    LoginPage user = new LoginPage(new LoginViewModel());
                    NavigationService.Navigate(user);
                }
                else
                {
                    MessageBox.Show("Non è possibile effettuare il logout");
                }
            }
            
        }

        private void UserPageLoaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation d = new DoubleAnimation();
            d.From = 0;
            d.To = 1062;
            d.Duration = TimeSpan.FromSeconds(1);
            d.EasingFunction = new QuadraticEase();
            Out.BeginAnimation(WidthProperty, d);
        }
    }
}
