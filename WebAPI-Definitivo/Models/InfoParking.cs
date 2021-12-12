using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebAPI_Definitivo.Models
{
    public partial class InfoParking
    {
        public InfoParking()
        {
            Parking = new HashSet<Parking>();
        }

        public InfoParking(string nomeParcheggio, int righe, int colonne)
        {
            NamePark = nomeParcheggio;
            Nrighe = righe;
            Ncol = colonne;
        }

        public long InfoParkId { get; set; }
        public string NamePark { get; set; }
        public int Ncol { get; set; }
        public int Nrighe { get; set; }

        public virtual ICollection<Parking> Parking { get; set; }
    }
}
