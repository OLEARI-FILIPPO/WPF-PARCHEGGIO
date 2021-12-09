﻿using HandyControl.Tools.Extension;
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
            

            if (UsernameTextBox.Text == "" || PasswordInserito.Password == "")
            {
                UsernameTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                PasswordInserito.BorderBrush = new SolidColorBrush(Colors.Red);
                MessageBox.Show("Inserire tutti i dati richiesti", "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
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

                    login.Token = result; //mi salvo il token

                    NavigationLoginToLogout.Token = result;

                    if (response.IsSuccessStatusCode)
                    {
                        NavigationLoginToLogout.result = result; //Ho creato una classe nella cartella Helper del progetto utilizzo questa classe per salvare lo stato della pagina
                        NavigationLoginToLogout.isLoggedIn = true;

                        //Rimuovo il login

                        //Aggiungo menu
                        ShellViewModel.MenuItems.Add
                        (
                            new HamburgerMenuGlyphItem() { Label = Properties.Resources.ShellMainPage, Glyph = "\uE80F", TargetPageType = typeof(MainViewModel) }
                        );

                        //Se il privilegio è 1 la vedo altrimenti no

                        ShellViewModel.MenuItems.Add
                        (
                            new HamburgerMenuGlyphItem() { Label = Properties.Resources.ShellParcheggiPage, Glyph = "\uE804", TargetPageType = typeof(ParcheggiViewModel) }
                        );
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
                DoubleAnimation d = new DoubleAnimation();
                d.From = 0;
                d.To = 600;
                d.Duration = TimeSpan.FromSeconds(1);
                d.EasingFunction = new QuadraticEase();
                log.BeginAnimation(HeightProperty, d);
            }
            
        }

       

    }
}
