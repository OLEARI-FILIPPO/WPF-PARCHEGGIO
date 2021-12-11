using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebAPI_Definitivo.Model
{
    public partial class History
    {
        public long HistoryId { get; set; }
        public long Id { get; set; }
        public string ParkingId { get; set; }
        public bool Stato { get; set; }
        public decimal? Revenue { get; set; }
        public DateTime? EntryTimeDate { get; set; }
        public long? VehicleId { get; set; }
        public DateTime? ExitTimeDate { get; set; }
        public long InfoParkId { get; set; }
        public string Token { get; set; }
        public DateTime? SearchDate { get; set; }
    }
}
