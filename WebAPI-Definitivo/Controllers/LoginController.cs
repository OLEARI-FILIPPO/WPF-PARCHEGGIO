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
                    Expires = DateTime.Now.AddDays(1),
                    Subject = new ClaimsIdentity(
                        new Claim[] 
                        { 
                            new Claim("Username", candidate.Username.ToString()) ,
                            new Claim("Grado", candidate.Grado.ToString()) ,     //privilegio
                            new Claim("Id", candidate.Id.ToString())      
                        })
                };
                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
                candidate.LastLogin = DateTime.Now;
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
        
        [Authorize]
        [HttpGet("ModifyUser/{username}/{password}/{newUsername}/{newPassword}")]
        public ActionResult ModifyUser(string username, string password, string newUsername, string newPassword)
        {
            using (ParkingManagementContext model = new ParkingManagementContext())
            {
                Users candidate = model.Users.Where(w => w.Username == username && w.Password == password).FirstOrDefault();
                Users candidate2 = model.Users.Where(w => w.Username == newUsername).FirstOrDefault();

                if(newUsername == username)
                    return Problem("L'username attuale è la stessa del nuovo");
                if (newPassword == username)
                    return Problem("La password attuale è la stessa della nuova");
                if (newUsername == "")
                    return Problem("Inserire correttamente l'username");
                if (newPassword == "")
                    return Problem("Inserire correttamente la password");

                if (candidate2 == null)
                {
                    candidate.Username = newUsername;
                    candidate.Password = newPassword;
                    candidate.LastLogout = DateTime.UtcNow.AddHours(1);
                    model.SaveChanges();
                    return Ok("Credenziali aggiornate correttamente");
                }
                else
                    return Problem("Username già utilizzato");
            }
        }

        [Authorize]
        [HttpGet("GetLogin/{username}/{password}")]
        public ActionResult GetLogin(string username, string password)
        {
            using (ParkingManagementContext model = new ParkingManagementContext())
            {
                Users candidate = model.Users.Where(w => w.Username == username && w.Password == password).FirstOrDefault();
                

                if (candidate != null)
                {
                    return Ok(candidate);
                }
                else
                    return NotFound();
            }
        }
    }
}
