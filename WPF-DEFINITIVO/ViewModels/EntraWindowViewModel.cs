using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace WPF_DEFINITIVO.ViewModels
{
    public class EntraWindowViewModel:ObservableObject
    {
        public EntraWindowViewModel()
        {

        }

        public static string surname;
        public static string name;
        public static DateTime dateBirth;

        public string Surname
        {
            get { return surname; }
            set
            {
                surname = value;
                OnPropertyChanged("Username");
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Username");
            }
        }

        public DateTime DateBirth
        {
            get { return dateBirth; }
            set
            {
                dateBirth = value;
                OnPropertyChanged("Username");
            }
        }
    }
}
