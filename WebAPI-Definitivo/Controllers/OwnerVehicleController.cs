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
    public class OwnerVehicleController : Controller
    {
        [Authorize]
        [HttpGet("/api/v1/OwnerVehicleList")]

        public ActionResult GetOwnerVehicleList()
        {
            try
            {

                using (ParkingManagementContext model = new ParkingManagementContext())
                {
                    List<OwnerVehicle> listOwnerVehicle;

                    listOwnerVehicle = model.OwnerVehicle.ToList();

                    // int count = listOfParkings.Count;

                    return Ok(listOwnerVehicle);

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
