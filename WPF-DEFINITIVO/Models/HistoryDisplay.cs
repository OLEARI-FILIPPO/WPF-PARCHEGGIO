using System;
using System.Collections.Generic;
using System.Text;

namespace WPF_DEFINITIVO.Models
{
    public class HistoryDisplay
    {
        public HistoryDisplay(string parkingId, decimal? revenue, DateTime? entryTimeDate, string licencePlate, DateTime? exitTimeDate, string namePark)
        {
            ParkingId = parkingId;
            Revenue = revenue;
            EntryTimeDate = entryTimeDate;
            LicencePlate = licencePlate;
            ExitTimeDate = exitTimeDate;
            NamePark = namePark;
        }

        public string ParkingId { get; set; }
        public decimal? Revenue { get; set; }
        public DateTime? EntryTimeDate { get; set; }
        public string LicencePlate { get; set; }
        public DateTime? ExitTimeDate { get; set; }
        public string NamePark { get; set; }
    }
}
