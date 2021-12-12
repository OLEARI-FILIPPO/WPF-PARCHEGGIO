using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_Definitivo.Models;

namespace WebAPI_Definitivo.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class VehicleController : Controller
    {
        [Authorize]
        [HttpGet("VehicleList")]

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

        [Authorize]
        [HttpGet("getLicence/{idVeicoli}")]
        public ActionResult GetLicencePlate(List<int> idVeicoli)
        {
            try
            {
                using (ParkingManagementContext model = new ParkingManagementContext())
                {
                    List<string> targhe = new List<string>();
                    string i = "";
                    int c = 0;
                    foreach (var item in idVeicoli)
                    {
                        i = model.Vehicle.Where(w => w.VehicleId == item).FirstOrDefault().LicensePlate;
                        targhe.Add(i);
                        c++;
                    }

                    return Ok(targhe);
                }
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [Authorize]
        [HttpGet("getParkName/{infoId}")]
        public ActionResult GetParkName(List<int> infoId)
        {
            try
            {
                using (ParkingManagementContext model = new ParkingManagementContext())
                {
                    List<string> nomi = new List<string>();
                    string i = "";
                    int c = 0;
                    foreach (var item in infoId)
                    {
                        i = model.InfoParking.Where(w => w.InfoParkId == item).FirstOrDefault().NamePark;
                        nomi.Add(i);
                        c++;
                    }

                    return Ok(nomi);
                }
            }
            catch (Exception)
            {
                return Problem();
            }
        }
    }
}
