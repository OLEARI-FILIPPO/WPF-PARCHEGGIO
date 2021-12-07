using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using WebAPI_Definitivo.Models;

namespace WebAPI_Definitivo
{
    public class SecurityKeyGenerator
    {
        public static SecurityKey GetSecurityKey(Users user)
        {
            var key = Encoding.ASCII.GetBytes(Startup.MasterKey + user.LastLogout.ToString());
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
