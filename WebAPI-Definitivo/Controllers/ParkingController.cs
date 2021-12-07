﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_Definitivo.Models;

namespace WebAPI_Definitivo.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        //Prendo tutti i parcheggi memorizzati
        [Authorize]
        [HttpGet("/api/v1/parcheggio")]
        public ActionResult GetParking()
        {
            string usernameUtente = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Username").Value;
            string grado = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Grado").Value;
            using (ParkingManagementContext model = new ParkingManagementContext())
            {
                List<Parking> parcheggi = model.Parking.ToList();
                return Ok(parcheggi);
            }
        }
        public ActionResult AddParking()
        {
            return Ok();
        }
    }
}
