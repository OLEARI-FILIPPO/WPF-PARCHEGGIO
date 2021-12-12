using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class ParkingController : ControllerBase
    {
        //Prendo tutti i parcheggi memorizzati
        [Authorize]
        [HttpGet("parcheggio")]
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
                            return Unauthorized("permesso negato");
                        default:
                            return Unauthorized("permesso negato");
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
        [HttpGet("history/{giorno}/{mese}/{anno}")]
        public ActionResult History(int giorno, int mese, int anno)
        {
            try
            {
                using (ParkingManagementContext model = new ParkingManagementContext())
                {
                    string grado = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Grado").Value;
                    string username = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Username").Value;

                    //Grado uno vedo tutto
                    List<History> history = new List<History>(); 

                    if (giorno == 0) { history = model.History.ToList(); }
                    else { history = model.History.Where(w => w.EntryTimeDate.Value.Day == giorno && w.EntryTimeDate.Value.Month == mese && w.EntryTimeDate.Value.Year == anno).ToList(); }
                    


                    if (history == null) { return NotFound("Nessun parcheggio corrispondente."); }

                    
                    return Ok(history); 

                    //Query per prendere i parcheggi di uno specifico user




                    /*var userHistory = from storico in model.History
                                      join vehicle in model.Vehicle on storico.VehicleId equals vehicle.VehicleId
                                      join owner in model.OwnerVehicle on vehicle.OwnerId equals owner.OwnerId
                                      join user in model.Users on owner.UserId equals user.Id

                                      where user.Username == username
                                      select new
                                      {
                                          HistoryId     = storico.HistoryId,
                                          ID            = storico.Id,
                                          ParkingId     = storico.ParkingId,
                                          Stato         = storico.Stato,
                                          Revenue       = storico.Revenue,
                                          EntryTimeDate = storico.EntryTimeDate,
                                          VehicleId     = storico.VehicleId,
                                          ExitTimeDate  = storico.ExitTimeDate,
                                          InfoParkId    = storico.InfoParkId,
                                          Token         = storico.Token,
                                          SearchDate    = storico.SearchDate
                                      };

                    if (userHistory.ToList() == null) { return NotFound("Nessun parcheggio corrispondente."); }
                    */
                    //return Ok(userHistory.ToList());
                }
            }
            catch (Exception)
            {
                return Problem();
            }
        }


        [Authorize]
        [HttpPost("history")]
        public ActionResult AddHistory([FromBody] History storico)
        {
            try
            {
                using (ParkingManagementContext model = new ParkingManagementContext())
                {
                    //Aggiorno history
                    model.History.Add(storico);
                    model.SaveChanges();

                    return Ok();
                }
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [Authorize]
        [HttpGet("parking-from-id/{parkId}/{infoParkName}")]       //post per passare il body
        public ActionResult ParkFromId(string parkId, string infoParkName)
        {
            try
            {
                using (ParkingManagementContext model = new ParkingManagementContext())
                {
                    var info = model.InfoParking.FirstOrDefault(f => f.NamePark == infoParkName);//oggetto del parcheggio selezionato

                    Parking parkingFound = model.Parking.FirstOrDefault(f => f.ParkingId == parkId && f.InfoParkId == info.InfoParkId && f.Stato == true);
                    if(parkingFound == null) { return NotFound("parcheggio non trovato"); }
                    parkingFound.InfoPark = null;//Senno va in loop e crasha

                    return Ok(parkingFound);
                }
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [Authorize]
        [HttpPut("parcheggio/{nomeParcheggio}/{nomePosto}")]
        //Reimposto i valori nella tabella parking
        public ActionResult UpdateParking(string nomeParcheggio, string nomePosto)
        {
            try
            {
                using (ParkingManagementContext model = new ParkingManagementContext())
                {
                    //trovo park id
                    var infoParkId = model.InfoParking.Where(w => w.NamePark == nomeParcheggio).FirstOrDefault();


                    //Update Parking Record
                    Parking parking = model.Parking.Where(w => w.ParkingId == nomePosto && w.InfoParkId == infoParkId.InfoParkId).FirstOrDefault();

                    parking.Stato = false;
                    parking.VehicleId = null;
                    parking.EntryTimeDate = null;
                    model.SaveChanges();

                    return Ok("Update riuscito");
                }
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        [Authorize]
        [HttpPut("parcheggio/{targa}/{nomeParcheggio}/{nomePosto}/{nomeManufactorer}/{nomeModel}")]
        //l'id è riferito al nome del parcheggio che l'utente ha cliccato
        public ActionResult AddParking(string targa, string nomeParcheggio, string nomePosto, string nomeManufactorer, string nomeModel, [FromBody] OwnerVehicle persona)
        {
            try
            {
                using (ParkingManagementContext model = new ParkingManagementContext())
                {
                    var id = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id").Value;
                    //Il super user per ora può aggiungere il veicolo con la persona
                    bool controlTarga = true;// variabile controllo corretto inserimento della targa
                    bool controlCognome = true;
                    bool controlNome = true;
                    bool controlManu = true;

                    targa = targa.ToUpper();
                    // Controllo il corretto inserimento della targa.
                    if (targa.Length != 7)
                        controlTarga = false;
                    for (int i = 0; i < targa.Length; i++)
                    {
                        int converAscii = (int)targa[i];
                        if (!controlTarga)
                            break;
                        if (i == 0 || i == 1 || i == 5 || i == 6)           // controllo che le prime e le ultime cifre della targa siano lettere
                        {
                            if (converAscii < 65 || converAscii > 90)
                            {
                                controlTarga = false;
                                break;
                            }
                        }
                        if (i == 2 || i == 3 || i == 4)
                            if (converAscii < 48 || converAscii > 57)     // controllo che le cifre centrali della targa siano numeri
                            {
                                controlTarga = false;
                                break;
                            }
                    }

                    if(!controlTarga)
                    {
                        return Problem("Inserire correttamente la targa");
                    }

                    for (int i = 0; i < persona.Name.Length; i++)
                    {
                        int converAscii = (int)persona.Name[i];
                        if (!controlNome)
                            break;

                        if (converAscii < 65 || converAscii > 90 && converAscii < 97 || converAscii > 122)
                        {
                            controlNome = false;
                            break;
                        }

                    }
                    for (int i = 0; i < persona.Surname.Length; i++)
                    {
                        int converAscii = (int)persona.Surname[i];
                        if (!controlCognome)
                            break;

                        if (converAscii < 65 || converAscii > 90 && converAscii < 97 || converAscii > 122)
                        {
                            controlCognome = false;
                            break;
                        }

                    }

                    if (!controlNome)
                        return Problem("Inserire correttamente il nome");
                    if (!controlCognome)
                        return Problem("Inserire correttamente il cognome");

                    for (int i = 0; i < nomeManufactorer.Length; i++)
                    {
                        int converAscii = (int)nomeManufactorer[i];
                        if (!controlCognome)
                            break;

                        if (converAscii < 65 || converAscii > 90 && converAscii < 97 || converAscii > 122)
                        {
                            controlManu = false;
                            break;
                        }

                    }

                    if (!controlManu)
                        return Problem("Inserire correttamente la marca");

                    //Inserimento veicolo
                    Vehicle controlloVeicolo = model.Vehicle.Where(w => w.LicensePlate == targa).FirstOrDefault();
                    int tempoAnni = DateTime.Now.Year - persona.DateBirth.Year;
                    int tempoMesi = DateTime.Now.Month - persona.DateBirth.Month;
                    int tempoGiorni = DateTime.Now.Day - persona.DateBirth.Day;
                    bool controlloEta = true;

                    if (tempoAnni < 14)
                        controlloEta = false;

                    if (tempoAnni == 14 && tempoMesi < 0)
                        controlloEta = false;

                    if (tempoAnni == 14 && tempoMesi == 0 && tempoGiorni < 0)
                        controlloEta = false;

                    if (controlloVeicolo == null && controlloEta)
                    {
                        persona.UserId = Convert.ToInt32(id);
                        model.OwnerVehicle.Add(persona);
                        model.SaveChanges();
                        Vehicle nuovoVeicolo = new Vehicle
                        (
                            licensePlate: targa,
                            ownerId: persona.OwnerId
                        );
                        model.Vehicle.Add(nuovoVeicolo);
                        model.SaveChanges();
                        //trovo park id
                        InfoParking infoParkId = model.InfoParking.Where(w => w.NamePark == nomeParcheggio).FirstOrDefault();
                        //Update Parking Record
                        Parking parking = model.Parking.Where(w => w.ParkingId == nomePosto && w.InfoParkId == infoParkId.InfoParkId).FirstOrDefault();

                        parking.Stato = true;
                        parking.EntryTimeDate = DateTime.Now;
                        parking.VehicleId = nuovoVeicolo.VehicleId;
                        model.SaveChanges();

                        return Ok("Update riuscito");
                    }
                    else if(controlloVeicolo != null)
                    {
                        string risposta = "L'auto targata " + targa + "è già presente nel parcheggio.";
                        return Problem("L'auto da parcheggiare è già presente nel parcheggio.");

                    }
                    else
                    {
                        return Problem("Il parcheggio è prenotabile sono da utenti con almeno 14 anni");
                    }

                }
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        
        
        [Authorize]
        [HttpGet("ParkingList")]

        public ActionResult GetParkingList()
        {
            try
            {

                using (ParkingManagementContext model = new ParkingManagementContext())
                {
                    List<InfoParking> listOfParkings;

                    listOfParkings = model.InfoParking.ToList();

                   // int count = listOfParkings.Count;

                    return Ok(listOfParkings);

                }

               // return Ok();
            }
            catch (Exception)
            {

                return Problem();
            }
        }

        [Authorize]
        [HttpGet("ParkingRecords")] //prende tutti i record tabella parking

        public ActionResult GetParkingRecords()
        {
            try
            {

                using (ParkingManagementContext model = new ParkingManagementContext())
                {
                    List<Parking> listOfParkings;

                    listOfParkings = model.Parking.ToList();

                    // int count = listOfParkings.Count;

                    return Ok(listOfParkings);

                }

                // return Ok();
            }
            catch (Exception)
            {

                return Problem();
            }
        }

        [Authorize]
        [HttpGet("/api/v1/ParkingRecordsByName/{nomeParcheggio}")] //prende tutti i record tabella parking
        public ActionResult GetParkingRecordsSingle(string nomeParcheggio)
        {
            try
            {
                //MODIFICA: SI PUO ANCHE SOLO PASSARE IL NOME DEL PARCHEGGIO INVECE CHE TUTTO L'OGGETTO
                using (ParkingManagementContext model = new ParkingManagementContext())
                {
                    List<Parking> listOfParkings;
                    long id;

                    id = model.InfoParking.Where(w => w.NamePark == nomeParcheggio).Select(s => s.InfoParkId).FirstOrDefault();
                    listOfParkings = model.Parking.Where(w => w.InfoParkId == id).ToList();

                    return Ok(listOfParkings);
                }
            }
            catch (Exception)
            {

                return Problem();
            }
        }


        [Authorize]
        [HttpGet("NotParked")] //prende i parcheggi vuoti
        public ActionResult GetNotParkedParking()
        {
            try
            {
                //MODIFICA: SI PUO ANCHE SOLO PASSARE IL NOME DEL PARCHEGGIO INVECE CHE TUTTO L'OGGETTO
                using (ParkingManagementContext model = new ParkingManagementContext())
                {
                    List<Parking> listOfParkings;
                    
                    listOfParkings = model.Parking.Where(s => s.Stato == false).ToList();

                    return Ok(listOfParkings);
                }
            }
            catch (Exception)
            {

                return Problem();
            }
        }

        [Authorize]
        [HttpGet("getVehicle")] //prende i parcheggi vuoti
        public ActionResult GetVehicle()
        {
            try
            {
                //MODIFICA: SI PUO ANCHE SOLO PASSARE IL NOME DEL PARCHEGGIO INVECE CHE TUTTO L'OGGETTO
                using (ParkingManagementContext model = new ParkingManagementContext())
                {
                    List<Vehicle> listOfVehicle;

                    listOfVehicle = model.Vehicle.ToList();

                    return Ok(listOfVehicle);
                }
            }
            catch (Exception)
            {

                return Problem();
            }
        }

        [Authorize]
        [HttpGet("NewPark/{nomeParcheggio}/{righe}/{colonne}")] //prende i parcheggi vuoti
        public ActionResult NewPark(string nomeParcheggio, string righe, string colonne)
        {
            try
            {
                //MODIFICA: SI PUO ANCHE SOLO PASSARE IL NOME DEL PARCHEGGIO INVECE CHE TUTTO L'OGGETTO
                using (ParkingManagementContext model = new ParkingManagementContext())
                {
                    int nRighe = Int32.Parse(righe);
                    int nColonne = Int32.Parse(colonne);
                    if (nomeParcheggio != "" && nRighe >= 2 && nColonne >= 2 && nRighe <= 10 && nColonne <= 10)
                    {
                        InfoParking pInfo = new InfoParking(nomeParcheggio, nRighe, nColonne);
                        model.InfoParking.Add(pInfo);
                        model.SaveChanges();

                        
                        int cont = 0;
                        for(int i = 0; i < nRighe; i++)
                        {
                            for (int j = 0; j < nColonne; j++)
                            {
                                Parking p = new Parking();
                                if (cont < 10)
                                    p.ParkingId = "P0" + cont;
                                else
                                    p.ParkingId = "P" + cont;
                                p.Stato = false;
                                p.InfoParkId = pInfo.InfoParkId;
                                cont++;

                                model.Parking.Add(p);
                                model.SaveChanges();
                            }
                        }
                        model.SaveChanges();
                        return Ok("Parcheggio creato correttamente selezionare il parcheggio...");
                    }
                    else
                        return Problem("Inserire correttamente tutti i campi");
                }
            }
            catch (Exception)
            {

                return Problem();
            }
        }

        //public Action

        [Authorize]
        [HttpGet("UsersInfo")]
        public ActionResult GetUsers()
        {
            try
            {
                
                using (ParkingManagementContext model = new ParkingManagementContext())
                {
                    List<Users> listOfUser;

                    listOfUser = model.Users.ToList();

                    return Ok(listOfUser);
                }
            }
            catch (Exception)
            {

                return Problem();
            }
        }

        [Authorize]
        [HttpGet("Incassi")]
        public ActionResult GetIncassi()
        {
            try
            {

                using (ParkingManagementContext model = new ParkingManagementContext())
                {
                    decimal somma = (decimal)model.Parking.Sum(s => s.Revenue);

                    return Ok(somma);
                }
            }
            catch (Exception)
            {

                return Problem();
            }
        }
    }
}
