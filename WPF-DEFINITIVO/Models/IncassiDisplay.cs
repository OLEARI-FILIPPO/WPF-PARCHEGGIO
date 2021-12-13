using System;
using System.Collections.Generic;
using System.Text;
using WebAPI_Definitivo.Models;

namespace WPF_DEFINITIVO.Models
{
    public class IncassiDisplay
    {
        public IncassiDisplay() { }
        public IncassiDisplay(string nomeParcheggio, int righe, int colonne, decimal? revenue)
        {
            NamePark = nomeParcheggio;
            Nrighe = righe;
            Ncol = colonne;
            Revenue = revenue;
        }
        public string NamePark { get; set; }
        public int Ncol { get; set; }
        public int Nrighe { get; set; }

        public decimal? Revenue { get; set; }

    }
}
