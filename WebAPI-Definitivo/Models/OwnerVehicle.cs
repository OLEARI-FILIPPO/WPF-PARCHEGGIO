using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebAPI_Definitivo.Models
{
    public partial class OwnerVehicle
    {
        public OwnerVehicle()
        {
            Vehicle = new HashSet<Vehicle>();
        }

        public long OwnerId { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public DateTime DateBirth { get; set; }
        public int? UserId { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<Vehicle> Vehicle { get; set; }
    }
}
