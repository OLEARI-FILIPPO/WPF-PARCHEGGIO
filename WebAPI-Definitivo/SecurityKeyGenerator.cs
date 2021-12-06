using System;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI_Definitivo.Models;
using WebAPI_Definitivo;

namespace WebAPI_Definitivo
{
    public class SecurityKeyGenerator
    {
        public static SecurityKey GetSecurityKey(Users user)
        {
            var key = Encoding.ASCII.GetBytes(Startup.MasterKey);
            return new SymmetricSecurityKey(key);
        }
        public static SecurityKey GetSecurityKey(string userName)
        {
            using (ParkingManagementContext model = new ParkingManagementContext())
            {
                Users user = model.Users.FirstOrDefault(q => q.Username == userName);
                return GetSecurityKey(user);
            }
        }
    }
}
