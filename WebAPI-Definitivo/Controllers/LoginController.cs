using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI_Definitivo.Models;

namespace WebAPI_Definitivo.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost("Login")]
        public ActionResult Login([FromBody] Users credentials)
        {
            using (ParkingManagementContext model = new ParkingManagementContext())
            {
                Users candidate = model.Users.FirstOrDefault(q => q.Username == credentials.Username && q.Password == credentials.Password);
                if (candidate == null) return NotFound("Username o password errati");
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    SigningCredentials = new SigningCredentials(SecurityKeyGenerator.GetSecurityKey(candidate),
                    SecurityAlgorithms.HmacSha256Signature),
                    Expires = DateTime.UtcNow.AddDays(1),
                    Subject = new ClaimsIdentity(
                        new Claim[] 
                        { 
                            new Claim("Username", candidate.Username.ToString()) ,
                            new Claim("Grado", candidate.Grado.ToString())      //privilegio
                        })
                };
                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
                model.SaveChanges();
                return Ok(tokenHandler.WriteToken(token));
            }
        }

        [HttpPost("GetToken")]
        public ActionResult GetToken([FromBody] Users credentials)
        {
            using (ParkingManagementContext model = new ParkingManagementContext())
            {
                Users candidate = model.Users.FirstOrDefault(q => q.Username == credentials.Username && q.Password == credentials.Password);
                if (candidate == null) return NotFound("Username o password errati");
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    SigningCredentials = new SigningCredentials(SecurityKeyGenerator.GetSecurityKey(candidate),
                    SecurityAlgorithms.HmacSha256Signature),
                    Expires = DateTime.UtcNow.AddDays(1),
                    Subject = new ClaimsIdentity(
                        new Claim[] 
                        {
                            new Claim("Username", candidate.Username.ToString()),
                            new Claim("Grado", candidate.Grado.ToString())
                        })
                };
                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
                model.SaveChanges();
                return Ok(tokenHandler.WriteToken(token));
            }
        }

        [Authorize]
        [HttpGet("GetUsers")]
        public ActionResult GetUsers([FromBody] Users credentials)
        {
            using (ParkingManagementContext model = new ParkingManagementContext())
            {
                List <Users> candidate = model.Users.ToList();
                return Ok(candidate);
            }
        }

        [Authorize]
        [HttpPost("Logout")]
        public ActionResult Logout([FromBody] Users credentials)
        {
            //prendo l'username attraverso il claim
            string usernameUtente = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Username").Value;

            using (ParkingManagementContext model = new ParkingManagementContext())
            {
                Users candidate = model.Users.FirstOrDefault(q => q.Username == usernameUtente); if (candidate == null) return NotFound();
                candidate.LastLogout = DateTime.Now;
                model.SaveChanges();
                return Ok();
            }
        }
    }
}
