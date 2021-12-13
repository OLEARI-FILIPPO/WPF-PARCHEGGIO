using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebAPI_Definitivo.Models
{
    public partial class History
    {
        public History() { }
        public History(long id, string parkingId, bool stato, decimal? revenue, DateTime? entryTimeDate, long? vehicleId, DateTime? exitTimeDate, long infoParkId)
        {
            Id = id;
            ParkingId = parkingId;
            Stato = stato;
            Revenue = revenue;
            EntryTimeDate = entryTimeDate;
            VehicleId = vehicleId;
            ExitTimeDate = exitTimeDate;
            InfoParkId = infoParkId;
        }

        public long HistoryId { get; set; }
        public long Id { get; set; }
        public string ParkingId { get; set; }
        public bool Stato { get; set; }
        public decimal? Revenue { get; set; }
        public DateTime? EntryTimeDate { get; set; }
        public long? VehicleId { get; set; }
        public DateTime? ExitTimeDate { get; set; }
        public long InfoParkId { get; set; }
    }
}
