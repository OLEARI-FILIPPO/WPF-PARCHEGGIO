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
using System.Windows.Media.Animation;
using System;
using System.Windows.Media;
using WPF_DEFINITIVO.Helpers;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;

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
            bool check = false;
       
      



            if (UsernameTextBox.Text == "Username" || PasswordInserito.Password == "password")
            {
                UsernameTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                PasswordInserito.BorderBrush = new SolidColorBrush(Colors.Red);
                

                if (MessageBox.Show("Inserire tutti i dati richiesti", "Alert", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    UsernameTextBox.BorderBrush = new SolidColorBrush(Colors.Transparent);
                }
            }
            else
            {
                UsernameTextBox.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#70E0E1");
                PasswordInserito.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#70E0E1");

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


                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                       MessageBox.Show("Username o password errarti", "Error",MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (response.IsSuccessStatusCode)
                    {
                        //Ottengo il grado traducendo il token che mi arriva dalla Richiesta post
                        var handler = new JwtSecurityTokenHandler();
                        var token = handler.ReadJwtToken(result);
                        string grado = token.Claims.FirstOrDefault(x => x.Type == "Grado").Value;

                        login.Token = result; //mi salvo il token

                        NavigationLoginToLogout.UserPriviledge = Convert.ToInt32(grado);

                        NavigationLoginToLogout.Token = result;

                        NavigationLoginToLogout.result = result; //Ho creato una classe nella cartella Helper del progetto utilizzo questa classe per salvare lo stato della pagina
                        NavigationLoginToLogout.isLoggedIn = true;
                   
                        //Rimuovo il login

                        //Aggiungo menu
                        ShellViewModel.MenuItems.Add
                        (
                            new HamburgerMenuGlyphItem() { Label = Properties.Resources.ShellMainPage, Glyph = "\uE80F", TargetPageType = typeof(MainViewModel) }
                        );

                        //Se il privilegio è 1 la vedo altrimenti no
                        if(grado == "1")
                        {
                            ShellViewModel.MenuItems.Add
                           (
                               new HamburgerMenuGlyphItem() { Label = Properties.Resources.ShellParcheggiPage, Glyph = "\uE804", TargetPageType = typeof(ParcheggiViewModel) }
                           );
                        }
                        
                      
                        ShellViewModel.MenuItems.Add
                        (
                            new HamburgerMenuGlyphItem() { Label = Properties.Resources.ShellStoricoPage, Glyph = "\uF738", TargetPageType = typeof(StoricoViewModel) }
                        );


                        UserPage user = new UserPage(new UserViewModel(credenziali, result));
                        NavigationLoginToLogout._user = credenziali;

                        NavigationService.Navigate(user);
                    }
                    else
                    {
                        MessageBox.Show(result);
                    }
                }
            }

            
        }

        private void LoginLoaded(object sender, RoutedEventArgs e)
        {

            if (NavigationLoginToLogout.isLoggedIn) //se l'utente è loggato allora navigo al userpage altrimenti avvio l'animazione del loginpage
            {
                UserPage _userpage = new UserPage(new UserViewModel(NavigationLoginToLogout._user, NavigationLoginToLogout.result));


                NavigationService.Navigate(_userpage);

            }
            else
            {
                Passwordtxt.Visibility = Visibility.Hidden;
                UsernameTextBox.Text = "Username";
                PasswordInserito.Password = "password";
                DoubleAnimation d = new DoubleAnimation();
                d.From = 0;
                d.To = 600;
                d.Duration = TimeSpan.FromSeconds(1);
                d.EasingFunction = new QuadraticEase();
                log.BeginAnimation(HeightProperty, d);
            }

          
            
        }

        private void PasswordInserito_GotFocus(object sender, RoutedEventArgs e)
        {

            if (PasswordInserito.Password == "password")
            {
                PasswordInserito.Password = null;
            }
            
        }

        private void PasswordInserito_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordInserito.Password == "")
            {
                PasswordInserito.Password = "password";
            }
        }

        private void UsernameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {

            if (UsernameTextBox.Text == "Username")
            {
                UsernameTextBox.Text = null;
            }
            
        }

        private void UsernameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if(UsernameTextBox.Text == "")
            {
                UsernameTextBox.Text = "Username";
            }
        }

        bool state = false;
        private void visible_Click(object sender, RoutedEventArgs e)
        {
            if (state == false)
            {
                Passwordtxt.Text = PasswordInserito.Password;
                Passwordtxt.Visibility = Visibility.Visible;
                PasswordInserito.Visibility = Visibility.Hidden;
                state = true;
            }
            else
            {

                Passwordtxt.Visibility = Visibility.Hidden;
                PasswordInserito.Visibility = Visibility.Visible;
                state = false;
            }
        }
    }
}
