using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json;
using WebAPI_Definitivo.Models;
using WPF_DEFINITIVO.Contracts.Services;
using WPF_DEFINITIVO.Contracts.ViewModels;
using WPF_DEFINITIVO.Core.Contracts.Services;
using WPF_DEFINITIVO.Core.Models;
using WPF_DEFINITIVO.Helpers;

namespace WPF_DEFINITIVO.ViewModels
{
    public class ParcheggiViewModel : ObservableObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private ObservableCollection<string> parkings = new ObservableCollection<string>();

        public ObservableCollection<string> Parking
        {
            get { return parkings; }
            set
            {
                this.parkings = value;
                OnPropertyChanged("parkings");
            }
        }

        private int riga;
        public int Riga
        {
            get { return riga; }
            set
            {
                this.riga = value;
                OnPropertyChanged("riga");
            }
        }


        private int colonna;
        public int Colonna
        {
            get { return colonna; }
            set
            {
                this.colonna = value;
                OnPropertyChanged("colonna");
            }
        }

        public List<InfoParking> ParkingObject;
        public async void GetParkings()
        {
            if (NavigationLoginToLogout.isLoggedIn)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);

                    var response = await client.GetAsync("http://localhost:13636/api/v1/ParkingList");
                    //await response.Content.ReadAsStringAsync();
                    var list = await response.Content.ReadAsStringAsync();
                    //response.Wait();

                    ParkingObject = JsonConvert.DeserializeObject<List<InfoParking>>(list);

                    //List<string> Parkings = new List<string>();

                    if (response.IsSuccessStatusCode)
                    {
                        parkings.Add("Nuovo-Parcheggio");

                        foreach (var item in ParkingObject)
                        {
                            parkings.Add(item.NamePark.ToString());
                        }
                    }
                    else
                    {
                        MessageBox.Show(list);
                    }



                    // var response = await client.GetAsync("http://localhost:13636/api/v1/ParkingList");
                }
            }
        }

        public List<Parking> ParkingObjectByName;
        public async Task GetParkingsByName(string nomeParcheggio)
        {
            if (NavigationLoginToLogout.isLoggedIn)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);
                    string url = "http://localhost:13636/api/v1/ParkingRecordsByName/" + nomeParcheggio;
                    var response = await client.GetAsync(url);
                    //await response.Content.ReadAsStringAsync();
                    var list = await response.Content.ReadAsStringAsync();
                    string ris = await response.Content.ReadAsStringAsync();

                    ParkingObjectByName = null;
                    //MessageBox.Show(ris);
                    if (response.IsSuccessStatusCode)
                    {
                        ParkingObjectByName = JsonConvert.DeserializeObject<List<Parking>>(list);
                    }
                    else
                    {
                        MessageBox.Show(list);
                    }


                }
            }
            //await Task.Delay(51);
        }


        public async Task GetRowColumn(string nomeParcheggio)
        {
            var tcs = new TaskCompletionSource<int>();
            if (NavigationLoginToLogout.isLoggedIn)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);

                    var response = await client.GetAsync("http://localhost:13636/api/v1/ParkingList");
                    //await response.Content.ReadAsStringAsync();
                    var list = await response.Content.ReadAsStringAsync();
                    //response.Wait();

                    if (response.IsSuccessStatusCode)
                    {

                        List<InfoParking> ParkingObject = JsonConvert.DeserializeObject<List<InfoParking>>(list);

                        foreach (var a in ParkingObject)
                        {
                            if (nomeParcheggio == a.NamePark)
                            {
                                Riga = a.Nrighe;
                                Colonna = a.Ncol;
                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show(list);
                    }
                    

                }
            }
           // await Task.Delay(51);

        }

        public List<Parking> parkingsRecords;
        public async Task GetParkingRecords(string nomeParcheggio)
        {
            if (NavigationLoginToLogout.isLoggedIn)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);

                    var response = await client.GetAsync("http://localhost:13636/api/v1/ParkingList");
                    //await response.Content.ReadAsStringAsync();
                    var list = await response.Content.ReadAsStringAsync();
                    //response.Wait();

                    string ID = string.Empty;

                    if (response.IsSuccessStatusCode)
                    {

                        List<InfoParking> ParkingObject = JsonConvert.DeserializeObject<List<InfoParking>>(list);
                        //MessageBox.Show(list);
                        foreach (var a in ParkingObject)
                        {
                            if (nomeParcheggio == a.NamePark)
                            {
                                ID = a.InfoParkId.ToString();
                            }

                        }

                        var responseParkingRecords = await client.GetAsync("http://localhost:13636/api/v1/ParkingRecords/"+ID);

                        var listParkingRecords = await responseParkingRecords.Content.ReadAsStringAsync();

                        parkingsRecords = JsonConvert.DeserializeObject< List<Parking> >(listParkingRecords);

                        //MessageBox.Show(listParkingRecords);

                    }
                    else
                    {
                        MessageBox.Show(list);
                    }


                }
            }
        }

        public async Task CreateParcheggio(string nomeParcheggio, int righe, int col)
        {
            if (NavigationLoginToLogout.isLoggedIn)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);

                    var response = await client.GetAsync("http://localhost:13636/api/v1/NewPark/" + nomeParcheggio + "/" + righe + "/" + col);
                    //await response.Content.ReadAsStringAsync();
                    var list = await response.Content.ReadAsStringAsync();
                    //response.Wait();
                    if (response.IsSuccessStatusCode)
                    {

                        MessageBox.Show(list);

                    }
                    else
                    {
                        MessageBox.Show(list);
                    }


                }
            }
        }




    }

}
