using System;
using System.ComponentModel;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using WPF_DEFINITIVO.Views;

namespace WPF_DEFINITIVO.ViewModels
{
    
    public class LoginViewModel : ObservableObject
    {

        public static bool isLoggedIn;
        public static UserPage userpage;
        public static string _Token;

        public LoginViewModel()
        {

        }

        public string username;
        public string password;
        public string token;
    

        public string Username
        {
            get { return username; }
            set
            {
                this.username = value;
                OnPropertyChanged("Username");
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                this.password = value;
                OnPropertyChanged("Password");
            }
        }

        public string Token
        {
            get { return this.token; }
            set
            {
                this.token = value;
                OnPropertyChanged("Token");
            }
        }

        

    }
}
