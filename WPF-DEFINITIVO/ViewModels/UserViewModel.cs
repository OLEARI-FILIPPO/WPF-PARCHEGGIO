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
        public UserViewModel(Users user)
        {
            Username = user.Username;
            Password = user.Password;
        }


    }
}
