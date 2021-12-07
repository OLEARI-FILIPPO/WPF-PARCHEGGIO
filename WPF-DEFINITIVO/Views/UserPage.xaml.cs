using System;
using System.Collections.Generic;
using System.Net.Http;
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
using WebAPI_Definitivo.Models;
using WPF_DEFINITIVO.ViewModels;

namespace WPF_DEFINITIVO.Views
{
    /// <summary>
    /// Logica di interazione per UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public UserPage(UserViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync("http://localhost:13636/api/v1/Logout", ""); //API controller name

                string result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Logout Confermato");
                    /*Menu menu = new Menu(Token, Username);
                    menu.Show();
                    this.Close();*/
                }
                else
                {
                    MessageBox.Show("Problema durante il logout.");
                }
            }
            LoginPage user = new LoginPage(new LoginViewModel());
            NavigationService.Navigate(user);
        }
    }
}
