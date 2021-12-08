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
            try
            {
                using (ParkingManagementContext model = new ParkingManagementContext())
                {
                    string grado = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Grado").Value;
                    List<Parking> parcheggi;
                    switch (grado) 
                    {
                        case "1":
                            parcheggi = model.Parking.ToList();
                            break;
                        case "2":
                            //user non può vedere per ora
                            return Unauthorized();
                        default:
                            return Problem("permesso negato");
                    }
                    return Ok(parcheggi);
                }
            }
            catch (Exception)
            {
                return Problem();
            }
            
        }

        [Authorize]
        [HttpPost("/api/v1/parcheggio/{id}")]
        //l'id è riferito al nome del parcheggio che l'utente ha cliccato
        public ActionResult AddParking(string id, [FromBody] OwnerVehicle persona)
        {
            try
            {
                using (ParkingManagementContext model = new ParkingManagementContext())
                {
                    string grado = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Grado").Value;
                    if(grado != "1") { return Unauthorized(); }

                    //Il super user per ora può aggiungere il veicolo con la persona
                    OwnerVehicle ownerVehicle = new OwnerVehicle
                        (
                            surname: persona.Surname,
                            name: persona.Name,
                            dateBirth: persona.DateBirth 
                        );
                    //Inserimento persona
                    model.OwnerVehicle.Add(ownerVehicle);
                    model.SaveChanges();

                    OwnerVehicle owner = model.OwnerVehicle.FirstOrDefault(l => l.Surname == persona.Surname && l.Name == persona.Name && l.DateBirth == persona.DateBirth);
                    if(owner == null) { return Problem("persona non trovata"); }

                    Vehicle nuovoVeicolo = new Vehicle
                        (
                            licensePlate: "GA007DN",        //dinamica
                            ownerId: owner.OwnerId
                        );
                    //Inserimento veicolo
                    model.Vehicle.Add(nuovoVeicolo);
                    model.SaveChanges();

                    //Parking record
                    Parking parking = new Parking
                        (
                            parkingId: id,
                            stato: true,
                            entryTimeDate: DateTime.UtcNow,
                            vehicleId: nuovoVeicolo.VehicleId,
                            infoParkId: 12  //dinamico
                        );
                    model.Parking.Add(parking);
                    model.SaveChanges();


                    return Ok("Inserimento riuscito");
                }
            }
            catch (Exception)
            {
                return Problem();
            }
        }
    }
}