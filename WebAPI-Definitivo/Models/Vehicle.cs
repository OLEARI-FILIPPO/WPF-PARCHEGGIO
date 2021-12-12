using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebAPI_Definitivo.Models
{
    public partial class Vehicle
    {
        public Vehicle()
        {
            
        }

        public Vehicle(string licensePlate, long ownerId)
        {
            LicensePlate = licensePlate;
            OwnerId = ownerId;
        }

        public long VehicleId { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string LicensePlate { get; set; }
        public long OwnerId { get; set; }

        public virtual OwnerVehicle Owner { get; set; }
        public virtual ICollection<Parking> Parking { get; set; }
    }
}
