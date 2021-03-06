using HandyControl.Data;
using HandyControl.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using WebAPI_Definitivo.Models;
using WPF_DEFINITIVO.Helpers;
using WPF_DEFINITIVO.ViewModels;

namespace WPF_DEFINITIVO.Views
{
    /// <summary>
    /// Logica di interazione per EntraWindow.xaml
    /// </summary>
    public partial class EntraWindow : Window, INotifyPropertyChanged
    {
        EntraWindowViewModel creazione;
        public EntraWindow(EntraWindowViewModel viewModel)
        {
            InitializeComponent();
            ConfigHelper.Instance.SetLang("it");
            DataContext = viewModel;
            creazione = viewModel;
        }

        private string startingDate = "01/01/2001";

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void entra_Click(object sender, RoutedEventArgs e)
        {
            //Se non c'è già un altra macchina allora proseguo

            //Inserisco il veicolo con la entry time date

            //Query inserimento persona

            if (MessageBox.Show("Sei sicuro di aver inserito correttamente i dati", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (Surname.Text == "Cognome" || Name.Text == "Nome" || LicensePlate.Text == "Targa" || Manufacturer.Text == "Marca" || Modello.Text == "Modello" || datePicker.SelectedDate == null)
                {

                    MessageBox.Show("Inserire tutti i dati richiesti", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Surname.BorderBrush = new SolidColorBrush(Colors.Red);
                    Name.BorderBrush = new SolidColorBrush(Colors.Red);
                    LicensePlate.BorderBrush = new SolidColorBrush(Colors.Red);
                    datePicker.BorderBrush = new SolidColorBrush(Colors.Red);
                    Manufacturer.BorderBrush = new SolidColorBrush(Colors.Red);
                    Modello.BorderBrush = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    Surname.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    Name.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    LicensePlate.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    datePicker.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    Manufacturer.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    Modello.BorderBrush = new SolidColorBrush(Colors.Transparent);

                    using (var client = new HttpClient())
                    {
                        int giorno = Int32.Parse(datePicker.Text.Split("/")[0]);
                        int mese = Int32.Parse(datePicker.Text.Split("/")[1]);
                        int anno = Int32.Parse(datePicker.Text.Split("/")[2]);

                        creazione.DateBirth = new DateTime(anno, mese, giorno);
                        OwnerVehicle parametri = new OwnerVehicle()
                        {
                            Surname = creazione.Surname,
                            Name = creazione.Name,
                            DateBirth = creazione.DateBirth

                        };


                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);

                        /// Chiamata API
                        string url = "http://localhost:13636/api/v1/parcheggio/" + creazione.Targa + "/" + creazione.nomeParcheggio + "/" + creazione.postoName + "/" + creazione.Manufactorer + "/" + creazione.Model;

                        //Chiamata
                        var response3 = await client.PutAsJsonAsync(url, parametri);
                        string result = await response3.Content.ReadAsStringAsync();



                        string[] errore = result.Split("auto");
                        string[] errore2 = result.Split("anni");
                        string[] errore3 = result.Split("cognome");
                        string[] errore4 = result.Split(" nome");
                        string[] errore5 = result.Split("targa");
                        string[] errore6 = result.Split("marca");

                        if (errore.Length > 1)
                            System.Windows.MessageBox.Show("La targa inserita è già presente tra i veicoli", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                       else if (errore2.Length > 1)
                            System.Windows.MessageBox.Show("Il parcheggio è prenotabile sono da utenti con almeno 14 anni", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                       else if (errore3.Length > 1)
                            System.Windows.MessageBox.Show("Inserire correttamente il cognome", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                       else if (errore4.Length > 1)
                            System.Windows.MessageBox.Show("Inserire correttamente il nome", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                       else if (errore5.Length > 1)
                            System.Windows.MessageBox.Show("Inserire correttamente la targa", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                      else  if (errore6.Length > 1)
                            System.Windows.MessageBox.Show("Inserire correttamente la marca", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                            this.Close();


                        // string error = result.Split(new string[] { })

                    }
                }
            }

            //this.Close();
                


         

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation d = new DoubleAnimation();
            d.From = 0;
            d.To = grid.ActualHeight;
            d.Duration = TimeSpan.FromSeconds(0.5);
            d.EasingFunction = new QuadraticEase();
            grid.BeginAnimation(HeightProperty, d);

            Manufacturer.Text = "Marca";
            Modello.Text = "Modello";

        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e) //TextBox Surname
        {
            if (Surname.Text == "Cognome")
            {
               Surname.Text = null;
            }

        }

        private void Name_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "Nome")
            {
                Name.Text = null;
            }

        }

        private void LicensePlate_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LicensePlate.Text == "Targa")
            {
                LicensePlate.Text = null;
            }

        }

        private void grid_Loaded(object sender, RoutedEventArgs e)
        {
            LicensePlate.Text = "Targa";
            Surname.Text = "Cognome";
            Name.Text = "Nome";

            
            datePicker.SelectedDate = null;
            datePicker.DisplayDateStart = Convert.ToDateTime(startingDate);
        }

        private void Surname_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Surname.Text == "")
            {
                Surname.Text = "Cognome";
            }
        }

        private void Name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "")
            {
                Name.Text = "Nome";
            }
        }

        private void LicensePlate_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LicensePlate.Text == "")
            {
                LicensePlate.Text = "Targa";
            }
        }

        private void Manufacturer_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Manufacturer.Text == "")
            {
                Manufacturer.Text = "Marca";
            }
        }

        private void Manufacturer_GotFocus(object sender, RoutedEventArgs e)
        {

            if (Manufacturer.Text == "Marca")
            {
                Manufacturer.Text = null;
            }
           
        }

        private void Modello_LostFocus(object sender, RoutedEventArgs e)
        {
            if(Modello.Text == "")
            {
               Modello.Text = "Modello";
            }
        }

        private void Modello_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Modello.Text == "Modello")
            {
               Modello.Text = null;
            }

        }
    }
}
