using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json;
using WebAPI_Definitivo.Models;
using WPF_DEFINITIVO.Contracts.ViewModels;
using WPF_DEFINITIVO.Core.Contracts.Services;
using WPF_DEFINITIVO.Core.Models;
using WPF_DEFINITIVO.Helpers;

namespace WPF_DEFINITIVO.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {


        }
      //  private readonly ISampleDataService _sampleDataService;

        public ObservableCollection<Parking> SourceDisp { get; } = new ObservableCollection<Parking>();
        public ObservableCollection<Vehicle> Vehicle { get; } = new ObservableCollection<Vehicle> { new Vehicle() };


        public string nParking;
        public string nVehicle; 
        public ObservableCollection<Parking> ParkingObject;

        public async Task GetParking()
        {
            SourceDisp.Clear();

            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);

                var response = await client.GetAsync("http://localhost:13636/api/v1/NotParked");

                var data = await response.Content.ReadAsStringAsync();

                ParkingObject = JsonConvert.DeserializeObject<ObservableCollection<Parking>>(data);

                foreach (Parking item in ParkingObject)
                {
                    SourceDisp.Add(item);
                }
            }


            nParking = SourceDisp.Count.ToString();

        }

        private ObservableCollection<Vehicle> VehicleObject;

        public async Task getVehicle()
        {
            Vehicle.Clear();

            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", NavigationLoginToLogout.Token);

                var response = await client.GetAsync("http://localhost:13636/api/v1/getVehicle");

                var data = await response.Content.ReadAsStringAsync();

                VehicleObject = JsonConvert.DeserializeObject<ObservableCollection<Vehicle>>(data);

                foreach (Vehicle item in VehicleObject)
                {
                    Vehicle.Add(item);
                }
            }

            nVehicle = VehicleObject.Count.ToString();


        }

        public void OnNavigatedFrom()
        {
        }
    }
}
