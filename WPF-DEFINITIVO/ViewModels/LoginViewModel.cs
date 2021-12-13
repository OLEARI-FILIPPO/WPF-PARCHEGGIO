using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json;
using WebAPI_Definitivo.Models;
using WPF_DEFINITIVO.Helpers;
using WPF_DEFINITIVO.Views;

namespace WPF_DEFINITIVO.ViewModels
{
    
    public class LoginViewModel : ObservableObject
    {
        public LoginViewModel()
        {

        }

        private ObservableCollection<Users> UsersCollection = new ObservableCollection<Users>();

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
