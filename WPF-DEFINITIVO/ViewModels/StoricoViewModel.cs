using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json;
using WebAPI_Definitivo.Models;
using WPF_DEFINITIVO.Contracts.ViewModels;
using WPF_DEFINITIVO.Core.Contracts.Services;
using WPF_DEFINITIVO.Core.Models;
using WPF_DEFINITIVO.Helpers;
using WPF_DEFINITIVO.Models;

namespace WPF_DEFINITIVO.ViewModels
{
    public class StoricoViewModel : ObservableObject, INavigationAware
    {

        public DateTime DateBirth { get; set; }
        public ObservableCollection<string> LicencePlate { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> NamePark { get; set; } = new ObservableCollection<string>();

        private readonly ISampleDataService _sampleDataService;

        public ObservableCollection<History> Source { get; set; } = new ObservableCollection<History>();
        public object ParkingObject { get; private set; }

        public ObservableCollection<HistoryDisplay> HistoryDisplay { get; set; } = new ObservableCollection<HistoryDisplay>();


        public StoricoViewModel(ISampleDataService sampleDataService)
        {
            _sampleDataService = sampleDataService;
        }

        public async Task OnNavigatedTo(object parameter)
        {
            await GeneraHistory(parameter);
        }

        public async Task GeneraHistory(object parameter)
        {
            Source.Clear();
            HistoryDisplay.Clear();
            HistoryHelper.oggetto = parameter;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);

                var response = await client.GetAsync("http://localhost:13636/api/v1/history/" + HistoryHelper.giorno + "/" + HistoryHelper.mese + "/" + HistoryHelper.anno + "");
                var list = await response.Content.ReadAsStringAsync();

                var lista = JsonConvert.DeserializeObject<ObservableCollection<History>>(list);

                if (response.IsSuccessStatusCode)
                {
                    foreach (var item in lista)
                    {
                        Source.Add(item);
                    }
                }
                else
                {
                    MessageBox.Show("Nessuna cronologia presente.");
                }


                List<int> idVeicoli = new List<int>();

                foreach (var item in Source)
                {
                    idVeicoli.Add(Convert.ToInt32(item.VehicleId));
                }


                List<int> infoId = new List<int>();

                foreach (var item in Source)
                {
                    infoId.Add(Convert.ToInt32(item.InfoParkId));
                }

                string url = "http://localhost:13636/api/v1/getLicence";
                response = await client.PostAsJsonAsync(url, idVeicoli);
                list = await response.Content.ReadAsStringAsync();

                var collezione = JsonConvert.DeserializeObject<ObservableCollection<string>>(list);

                if (response.IsSuccessStatusCode)
                {
                    LicencePlate.Clear();
                    foreach (var item in collezione)
                    {
                        LicencePlate.Add(item);
                    }
                }

                url = "http://localhost:13636/api/v1/getParkName";
                response = await client.PostAsJsonAsync(url, infoId);
                list = await response.Content.ReadAsStringAsync();

                collezione = JsonConvert.DeserializeObject<ObservableCollection<string>>(list);

                if (response.IsSuccessStatusCode)
                {
                    NamePark.Clear();
                    foreach (var item in collezione)
                    {
                        NamePark.Add(item);
                    }


                    for (int i = 0; i < Source.Count; i++)
                    {
                        HistoryDisplay.Add
                            (
                                new HistoryDisplay
                                (
                                    Source[i].ParkingId,
                                    Source[i].Revenue,
                                    Source[i].EntryTimeDate,
                                    LicencePlate[i],
                                    Source[i].ExitTimeDate,
                                    NamePark[i]
                                )
                            );
                    }

                    if (HistoryDisplay.Count == 0)
                    {
                        HistoryDisplay displayEmpty = new HistoryDisplay();
                        displayEmpty.ParkingId = "NESSUNN VEICOLO TROVATO PER LA DATA INSERITA";
                        HistoryDisplay.Add(displayEmpty);
                        HistoryHelper.check = false;
                    }
                    else
                        HistoryHelper.check = true;

                }

                

                HistoryHelper.giorno = 0;
            }
        }

        public async Task Refresh()
        {
            //MessageBox.Show("fatto"); 
            //HistoryHelper.check
            await OnNavigatedTo(HistoryHelper.oggetto);
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
