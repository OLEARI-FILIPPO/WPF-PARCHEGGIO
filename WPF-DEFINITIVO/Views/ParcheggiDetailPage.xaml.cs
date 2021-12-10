using Newtonsoft.Json;
using System;
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
        string parkingName;
        bool stato;
        public ParcheggiDetailPage(string _parkingName, Button _button)
        {
            InitializeComponent();
            parkingName = _parkingName;
            buttonName = _button.Name.Split("btn")[1];
            if (_button.Background == (SolidColorBrush)new BrushConverter().ConvertFrom("#F77B7B"))
                stato = true;
            else
                stato = false;
            ParkingNameLabel.Text = parkingName;
            DataContext = this;

            // DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EntraWindow entraWindow = new EntraWindow(new EntraWindowViewModel());
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
                    // Creo lista di Parcheggi
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);
                    var response = await client.GetAsync("http://localhost:13636/api/v1/ParkingRecords");
                    //await response.Content.ReadAsStringAsync();
                    var list = await response.Content.ReadAsStringAsync();
                    //response.Wait();
                    ObservableCollection<Parking> ParkingObject = JsonConvert.DeserializeObject<ObservableCollection<Parking>>(list);
                    foreach(var a in ParkingObject)
                    {
                        if (a.Id.ToString() == buttonName)
                            DataEntrata.Text = a.EntryTimeDate.ToString();
                        else
                            DataEntrata.Text = "Data Entrata";
                    }

                    // Creo lista di Veicoli
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);
                    var response2 = await client.GetAsync("http://localhost:13636/api/v1/VehicleList");
                    //await response.Content.ReadAsStringAsync();
                    var list2 = await response2.Content.ReadAsStringAsync();
                    string ris = await response2.Content.ReadAsStringAsync();
                    MessageBox.Show(ris);
                    //response.Wait();
                    ObservableCollection<Vehicle> VehicleObject = JsonConvert.DeserializeObject<ObservableCollection<Vehicle>>(list);
                    foreach (var a in ParkingObject)
                    {
                        if (a.Id.ToString() == buttonName)
                        {
                            foreach (var b in VehicleObject)
                            {
                                if(a.VehicleId == b.VehicleId)
                                    Targa.Text = b.LicensePlate.ToString();
                                else
                                    DataEntrata.Text = "Targa";
                            }
                        }
                            
                    }

                    // Creo lista di ProprietaridiVeicoli
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);
                    var response3 = await client.GetAsync("http://localhost:13636//api/v1/OwnerVehicleList");
                    //await response.Content.ReadAsStringAsync();
                    var list3 = await response3.Content.ReadAsStringAsync();
                    //response.Wait();
                    ObservableCollection<OwnerVehicle> OwnerVehicleObject = JsonConvert.DeserializeObject<ObservableCollection<OwnerVehicle>>(list3);
                    foreach (var a in ParkingObject)
                    {
                        if (a.Id.ToString() == buttonName)
                        {
                            foreach (var b in VehicleObject)
                            {
                                if (a.VehicleId == b.VehicleId)
                                {
                                    foreach (var c in OwnerVehicleObject)
                                    {
                                        if (b.OwnerId == c.OwnerId)
                                            Proprietario.Text = c.Name.ToString() + " " + c.Surname.ToString();
                                        else
                                            DataEntrata.Text = "Proprietario";
                                    }
                                }
                                    
                            }
                        }

                    }




                    // var response = await client.GetAsync("http://localhost:13636/api/v1/ParkingList");
                }
            }
        }
    }
}
