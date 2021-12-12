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
        [HttpGet("getLicence/{source}")]
        public ActionResult GetLicencePlate(ObservableCollection<History> source)
        {
            try
            {
                using (ParkingManagementContext model = new ParkingManagementContext())
                {
                    ObservableCollection<string> targhe = new ObservableCollection<string>();
                    string i = "";
                    int c = 0;
                    foreach (var item in source)
                    {
                        i = model.Vehicle.Where(w => w.VehicleId == Convert.ToInt32(source[c].VehicleId)).FirstOrDefault().LicensePlate;
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
        [HttpGet("getParkName/{source}")]
        public ActionResult GetParkName(ObservableCollection<History> source)
        {
            try
            {
                using (ParkingManagementContext model = new ParkingManagementContext())
                {
                    ObservableCollection<string> nomi = new ObservableCollection<string>();
                    string i = "";
                    int c = 0;
                    foreach (var item in source)
                    {
                        i = model.InfoParking.Where(w => w.InfoParkId == Convert.ToInt32(source[c].InfoParkId)).FirstOrDefault().NamePark;
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
