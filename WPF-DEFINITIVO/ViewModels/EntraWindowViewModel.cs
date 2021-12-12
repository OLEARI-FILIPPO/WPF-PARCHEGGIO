using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace WPF_DEFINITIVO.ViewModels
{
    public class EntraWindowViewModel:ObservableObject
    {
        public string nomeParcheggio;
        public string postoName;
        public EntraWindowViewModel(string _nomeParcheggio, string _postoName)
        {
            nomeParcheggio = _nomeParcheggio;
            postoName = _postoName;
        }

        public EntraWindowViewModel()
        {

        }
        private string surname;
        private string name;
        private DateTime dateBirth;
        private string targa;
        private string manufactorer;
        private string model;

        public string Surname
        {
            get { return surname; }
            set
            {
                surname = value;
                OnPropertyChanged("surname");
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("name");
            }
        }

        public DateTime DateBirth
        {
            get { return dateBirth; }
            set
            {
                dateBirth = value;
                OnPropertyChanged("dateBirth");
            }
        }

        public string Targa
        {
            get { return targa; }
            set
            {
                targa = value;
                OnPropertyChanged("targa");
            }
        }

        public string Manufactorer
        {
            get { return manufactorer; }
            set
            {
                manufactorer = value;
                OnPropertyChanged("manufactorer");
            }
        }

        public string Model
        {
            get { return model; }
            set
            {
                model = value;
                OnPropertyChanged("model");
            }
        }

    }
}
