using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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
    public class ParcheggiViewModel : ObservableObject//, INavigationAware
    {

        private ObservableCollection<string> parkings = new ObservableCollection<string>();

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

                    List<InfoParking> ParkingObject = JsonConvert.DeserializeObject<List<InfoParking>>(list);

                    //List<string> Parkings = new List<string>();

                    parkings.Add("Nuovo-Parcheggio");

                    foreach (var item in ParkingObject)
                    {
                        parkings.Add(item.NamePark.ToString());
                    }

                 


                    // var response = await client.GetAsync("http://localhost:13636/api/v1/ParkingList");
                }
            }
          

        }

     //  public ObservableCollection<string> Parking { get { return GetParkings(); } }
    }
}
