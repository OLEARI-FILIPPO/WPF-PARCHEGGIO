using HandyControl.Tools.Extension;
using System.ComponentModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using Windows.System;
using WPF_DEFINITIVO.ViewModels;
using WPF_DEFINITIVO.Models;
using WebAPI_Definitivo.Models;
using System.Net.Http.Headers;

namespace WPF_DEFINITIVO.Views
{
    public partial class LoginPage : Page, INotifyPropertyChanged
    {
        LoginViewModel login;
        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            login = viewModel;


            //this.DataContext = this;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        static HttpClient httpClient2 = new HttpClient();

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            

            using (var client = new HttpClient())
            {
                Users credenziali = new Users()
                {
                    Username = login.Username,
                    Password = PasswordInserito.Password
                };

                //request.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{yourusername}:{yourpwd}")));
                
                var response = await client.PostAsJsonAsync("http://localhost:13636/api/v1/Login", credenziali); //API controller name

                string result = await response.Content.ReadAsStringAsync();

                login.Token = result; //mi salvo il token

                if (response.IsSuccessStatusCode)
                {
                    UserPage user = new UserPage(new UserViewModel(credenziali, result));
                    NavigationService.Navigate(user);
                }
                else
                {
                    MessageBox.Show(result);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
