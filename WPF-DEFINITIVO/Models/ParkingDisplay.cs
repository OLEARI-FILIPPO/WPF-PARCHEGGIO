using System;
using System.Collections.Generic;
using System.Text;

namespace WPF_DEFINITIVO.Models
{
    public class ParkingDisplay
    {
        public ParkingDisplay(string nomePosto, string stato, string nomeParcheggio)
        {
            NomePosto = nomePosto;
            Stato = stato;
            NomeParcheggio = nomeParcheggio;
        }

        public string NomePosto { get; set; }
        public string Stato { get; set; }
        public string NomeParcheggio { get; set; }
    }
}
