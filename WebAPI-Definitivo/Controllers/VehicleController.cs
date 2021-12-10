using Microsoft.AspNetCore.Authorization;
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
    public class VehicleController : Controller
    {
        [Authorize]
        [HttpGet("/api/v1/VehicleList")]

        public ActionResult GetVehicleList()
        {
            try
            {

                using (ParkingManagementContext model = new ParkingManagementContext())
                {
                    List<Vehicle> listVehicle;

                    listVehicle = model.Vehicle.ToList();

                    // int count = listOfParkings.Count;

                    return Ok(listVehicle);

                }

                // return Ok();
            }
            catch (Exception)
            {

                return Problem();
            }
        }
    }
}
