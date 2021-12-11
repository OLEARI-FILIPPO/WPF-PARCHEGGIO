using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF_DEFINITIVO.Views
{
    /// <summary>
    /// Logica di interazione per SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        public SignUp()
        {
            InitializeComponent();
            ConfigHelper.Instance.SetLang("it");
        }


        private void Username_GotFocus(object sender, RoutedEventArgs e) //TextBox Surname
        {
            Username.Text = null;
        }

        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            Password.Password = null;
        }

   
        private void grid_Loaded(object sender, RoutedEventArgs e)
        {
            
            Username.Text = "Cognome";
            Password.Password = "Password";

            Passwordtxt.Visibility = Visibility.Hidden;

          
        }

        private void Username_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Username.Text == "")
            {
                Username.Text = "Cognome";
            }
        }

        private void Password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Password.Password == "")
            {
                Password.Password = "Password";
            }
        }

        private void Crea_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Annulla_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

  
       

        private void visible_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Passwordtxt.Text = Password.Password;
            Passwordtxt.Visibility = Visibility.Visible;
            Password.Visibility = Visibility.Hidden;
        }


        bool state = false;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (state == false)
            {
                Passwordtxt.Text = Password.Password;
                Passwordtxt.Visibility = Visibility.Visible;
                Password.Visibility = Visibility.Hidden;
                state = true;
            }
            else
            {
               
                Passwordtxt.Visibility = Visibility.Hidden;
                Password.Visibility = Visibility.Visible;
                state = false;
            }
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation d = new DoubleAnimation();
            d.From = 0;
            d.To = grid.ActualHeight;
            d.Duration = TimeSpan.FromSeconds(1);
            d.EasingFunction = new QuadraticEase();
            grid.BeginAnimation(HeightProperty, d);
        }
    }
}
