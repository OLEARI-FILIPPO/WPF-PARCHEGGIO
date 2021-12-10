using HandyControl.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
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
            /* PARKING
             * [ID] [bigint] IDENTITY(1, 1) NOT NULL,
 
             [ParkingId] [char](3) NOT NULL,
 
             [Stato] [bit] NOT NULL,
 
             [Revenue] [decimal](18, 0) NULL,
	        [EntryTimeDate] [datetime] NULL,
	        [VehicleId] [bigint] NULL,
	        [InfoParkId] [bigint] NOT NULL,

            [Token] [char](5) NULL,*/

            //Query inserimento persona

            using (var client = new HttpClient())
            {
                OwnerVehicle parametri = new OwnerVehicle()
                {
                    Surname = creazione.Surname,
                    Name = creazione.Name,
                    DateBirth = creazione.DateBirth
                };
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);

                //Chiamo l'api per la creazione del parcheggio
                string url = "http://localhost:13636/api/v1/ParkingRecordsByName";
                InfoParking i = new InfoParking();
                i.InfoParkId = 1;
                i.Ncol = 1;
                i.Nrighe = 1;
                i.NamePark = creazione.nomeParcheggio;
                var response = await client.PostAsJsonAsync(url, i);
                var list = await response.Content.ReadAsStringAsync();
                List<Parking> ParkingObjectByName;
                if (response.IsSuccessStatusCode)
                {
                    ParkingObjectByName = JsonConvert.DeserializeObject<List<Parking>>(list);

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);
                    var response2 = await client.GetAsync("http://localhost:13636/api/v1/VehicleList");
                    //await response.Content.ReadAsStringAsync();
                    var list2 = await response2.Content.ReadAsStringAsync();
                    MessageBox.Show(list2);
                    //response.Wait();
                    if(response2.IsSuccessStatusCode)
                    {
                        List<Vehicle> VehicleObject = JsonConvert.DeserializeObject<List<Vehicle>>(list2);
                        MessageBox.Show(list2);

                        foreach (var a in ParkingObjectByName)
                        {
                            foreach (var b in VehicleObject)
                            {
                                if (a.VehicleId == b.VehicleId)
                                {
                                    if (b.LicensePlate == creazione.Targa)
                                    {
                                        MessageBox.Show("Targa Già Presente");
                                        goto EndOfLoop;
                                    }
                                    else
                                        MessageBox.Show(creazione.Targa);
                                }

                            }
                        }
                    }
                    


                }
                EndOfLoop:;


                // Chiamata API
            }




        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation d = new DoubleAnimation();
            d.From = 0;
            d.To = grid.ActualHeight;
            d.Duration = TimeSpan.FromSeconds(0.5);
            d.EasingFunction = new QuadraticEase();
            grid.BeginAnimation(HeightProperty, d);
        }
    }
}
