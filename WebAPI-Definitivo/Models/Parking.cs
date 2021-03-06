using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebAPI_Definitivo.Models
{
    public partial class Parking
    {
        public Parking(string parkingId, bool stato, DateTime entryTimeDate, long vehicleId, int infoParkId)
        {
            ParkingId = parkingId;
            Stato = stato;
            EntryTimeDate = entryTimeDate;
            VehicleId = vehicleId;
            InfoParkId = infoParkId;
        }

        public Parking()
        {

        }

        public long Id { get; set; }
        public string ParkingId { get; set; }
        public bool Stato { get; set; }
        public DateTime? EntryTimeDate { get; set; }
        public long? VehicleId { get; set; }
        public long InfoParkId { get; set; }

        public virtual InfoParking InfoPark { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}
