using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebAPI_Definitivo.Models
{
    public partial class Users
    {
        public Users()
        {
            OwnerVehicle = new HashSet<OwnerVehicle>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Grado { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? LastLogout { get; set; }

        public virtual ICollection<OwnerVehicle> OwnerVehicle { get; set; }
    }
}
