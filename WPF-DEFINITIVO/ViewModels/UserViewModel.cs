using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;
using WebAPI_Definitivo.Models;

namespace WPF_DEFINITIVO.ViewModels
{
    public class UserViewModel : ObservableObject
    {
        public string Username;
        public string Password;
        public string Token;
        public UserViewModel(Users user, string result)
        {
            Username = user.Username;
            Password = user.Password;
            Token = result;
        }


    }
}
