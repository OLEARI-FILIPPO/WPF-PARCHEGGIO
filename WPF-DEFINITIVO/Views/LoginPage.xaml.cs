using HandyControl.Tools.Extension;
using System.ComponentModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using Windows.System;
using WPF_DEFINITIVO.ViewModels;
using WPF_DEFINITIVO.Models;
using WebAPI_Definitivo.Models;
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
        static HttpClient client = new HttpClient();

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
                    Password = login.Password
                };
                
                var response = await client.PostAsJsonAsync("http://localhost:13636/api/v1/Login", credenziali); //API controller name

                string result = await response.Content.ReadAsStringAsync();

                login.Token = result; //mi salvo il token

                if (response.IsSuccessStatusCode)
                {
                    //MessageBox.Show("Login Confermato");
                    UserPage user = new UserPage(new UserViewModel(credenziali));
                    NavigationService.Navigate(user);
                }
                else
                {
                    MessageBox.Show("Problema durante l'accesso, forse le credenziali non sono valide");
                }
            }
        }

        private void PasswordUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            login.password = PasswordInserito.Password;
        }

        
    }
}
