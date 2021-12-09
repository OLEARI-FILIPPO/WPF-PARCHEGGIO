using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_Definitivo.Models;

namespace WebAPI_Definitivo.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ParkingAccessController : Controller
    {
        [HttpGet("GetInfoParking")]
        public ActionResult GetInfoParking()
        {
            using (ParkingManagementContext model = new ParkingManagementContext())
            {
                List <InfoParking> candidate = model.InfoParking.ToList();
                return Ok(candidate);
            }
        }
    }
}
