using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using WebAPI_Definitivo.Models;
using WPF_DEFINITIVO.Helpers;
using WPF_DEFINITIVO.ViewModels;

namespace WPF_DEFINITIVO.Views
{
    public partial class ParcheggiDetailPage : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        string buttonName;
        string postoName;
        string parkingName;
        string comboItemSelected;
        bool stato;
        public ParcheggiDetailPage(string _postoName, Button _button, string _parkingName, string _comboItemSelected)
        {
            InitializeComponent();

            comboItemSelected = _comboItemSelected;
            postoName = _postoName;
            parkingName = _parkingName;
            buttonName = _button.Name.Split("btn")[1];


            if (_button.Background.ToString() == "#FFF77B7B")
                stato = true;
            else
                stato = false;
            ParkingNameLabel.Text = postoName;
            DataContext = this;
            btnPosto.Foreground = new SolidColorBrush(Colors.Black);
            btnPosto.BorderThickness = new Thickness(1);
            if (stato)
            {
                Exit.IsEnabled = true;
                Enter.IsEnabled = false;
                btnPosto.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#F77B7B");
            }
            else
            {
                Exit.IsEnabled = false;
                Enter.IsEnabled = true;
                btnPosto.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#93C7EA");
            }

            storicoGrid.Visibility = Visibility.Hidden;
            textstorico.Visibility = Visibility.Hidden;
            detailPage.Height = 300;
            detailPage.MaxHeight = 301;


            // DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EntraWindow entraWindow = new EntraWindow(new EntraWindowViewModel(parkingName, postoName));
            this.Close();
            entraWindow.ShowDialog();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation d = new DoubleAnimation();
            d.From = 0;
            d.To = grid.ActualHeight;
            d.Duration = TimeSpan.FromSeconds(0.4);
            d.EasingFunction = new QuadraticEase();
            grid.BeginAnimation(HeightProperty, d);

            if (NavigationLoginToLogout.isLoggedIn)
            {
                using (var client = new HttpClient())
                {
                    if (stato)
                        statoPark.Text = "Occupato";
                    else
                        statoPark.Text = "Libero";
                    // Creo lista di Parcheggi
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);

                    var response = await client.GetAsync("http://localhost:13636/api/v1/ParkingRecordsByName/" + parkingName);
                    var list = await response.Content.ReadAsStringAsync();

                    ObservableCollection<Parking> ParkingObject = JsonConvert.DeserializeObject<ObservableCollection<Parking>>(list);
                    foreach (var a in ParkingObject)
                    {
                        if (a.ParkingId.ToString() == buttonName && a.EntryTimeDate != null)
                        {
                            DataEntrata.Text = a.EntryTimeDate.ToString();
                            break;
                        }
                        else
                            DataEntrata.Text = "Non ancora impostata";
                    }

                    // Creo lista di Veicoli
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);
                    var response2 = await client.GetAsync("http://localhost:13636/api/v1/VehicleList");

                    var list2 = await response2.Content.ReadAsStringAsync();
                    List<Vehicle> VehicleObject = JsonConvert.DeserializeObject<List<Vehicle>>(list2);
                    foreach (var a in ParkingObject)
                    {
                        if (a.ParkingId.ToString() == buttonName)
                        {
                            foreach (var b in VehicleObject)
                            {
                                if(a.VehicleId == b.VehicleId)
                                {
                                    Targa.Text = b.LicensePlate.ToString();
                                    goto EndOfLoop;
                                }
                                else
                                    Targa.Text = "Non ancora impostato";
                            }
                        }
                        else
                            Targa.Text = "Non ancora impostata";

                    }
                    EndOfLoop:;

                    // Creo lista di ProprietaridiVeicoli
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);

                    var response3 = await client.GetAsync("http://localhost:13636/api/v1/OwnerVehicleList");
                    var list3 = await response3.Content.ReadAsStringAsync();

                    ObservableCollection<OwnerVehicle> OwnerVehicleObject = JsonConvert.DeserializeObject<ObservableCollection<OwnerVehicle>>(list3);
                    foreach (var a in ParkingObject)
                    {
                        if (a.ParkingId.ToString() == buttonName)
                        {
                            foreach (var b in VehicleObject)
                            {
                                if (a.VehicleId == b.VehicleId)
                                {
                                    foreach (var c in OwnerVehicleObject)
                                    {
                                        if (b.OwnerId == c.OwnerId)
                                        {
                                            Proprietario.Text = c.Name.ToString() + " " + c.Surname.ToString();
                                            goto EndOfLoop2; ;
                                        }
                                        else
                                            Proprietario.Text = "Non ancora impostato";
                                    }
                                }
                                else
                                    Proprietario.Text = "Non ancora impostato";

                            }
                        }
                        else
                            Proprietario.Text = "Non ancora impostato";

                    }
                    EndOfLoop2:;
                }
            }
        }

        private async void Exit_Click(object sender, RoutedEventArgs e)
        {
            //USCITA DEL VEICOLO

            using (var client = new HttpClient())
            {
                //Autenticazione token
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);


                //Chiamo l'api per ottenere i campi di history 

                string url = "http://localhost:13636/api/v1/parking-from-id/" + postoName + "/" + comboItemSelected + "";

                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Veicolo non uscito.");
                }
                else
                {
                    Parking park = JsonConvert.DeserializeObject<Parking>(result);

                    DateTime OraEntrata = (DateTime)park.EntryTimeDate;
                    decimal costo = (decimal)(DateTime.UtcNow.AddHours(1) - OraEntrata).TotalHours;

                    if (costo < 1)
                        costo = 1;

                    decimal tariffa = 2;
                    //Oggetto da passare
                    History storico = new History
                        (
                            id: park.Id,            //id del parcheggio
                            parkingId: postoName,   //id temporaneo del parcheggio
                            stato: false,
                            revenue: costo * tariffa,            
                            entryTimeDate: park.EntryTimeDate,
                            vehicleId: park.VehicleId,
                            exitTimeDate: DateTime.UtcNow.AddHours(1),         
                            infoParkId: park.InfoParkId
                        );


                    //Chiamo l'api per la creazione del parcheggio
                    url = "http://localhost:13636/api/v1/history";

                    var response2 = await client.PostAsJsonAsync(url, storico);

                    if (response2.IsSuccessStatusCode)
                    {
                        url = "http://localhost:13636/api/v1/parcheggio/" + parkingName + "/" + postoName + "";

                        //Chiamata
                        var response3 = await client.PutAsJsonAsync(url, "");
                        result = await response.Content.ReadAsStringAsync();

                        if (response3.IsSuccessStatusCode)
                        {
                            Close();
                            MessageBox.Show("Veicolo uscito con successo.");
                        }
                        else
                            MessageBox.Show("Errore: parametri non aggiornati");
                    }
                    else
                    {
                        MessageBox.Show("Veicolo non uscito.");
                    }
                }
                
            }
        }
    }
}
