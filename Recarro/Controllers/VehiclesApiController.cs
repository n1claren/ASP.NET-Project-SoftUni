using Microsoft.AspNetCore.Mvc;
using Recarro.Data.Models;
using Recarro.Services.Vehicles;
using System.Collections.Generic;
using System.Linq;

namespace Recarro.Controllers
{
    [ApiController]
    [Route("api/vehicles")]
    public class VehiclesApiController : ControllerBase
    {
        private readonly IVehicleService vService;

        public VehiclesApiController(IVehicleService vService) 
            => this.vService = vService;

        [HttpGet]
        public ActionResult<List<Vehicle>> GetAllVehiclesData()
        {
            var vehicles = vService.GetAllVehicles();

            if (!vehicles.Any())
            {
                return NotFound();
            }

            return Ok(vehicles);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Vehicle> GetVehicleData(int id)
        {
            var vehicle = vService.GetVehicleById(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }
    }
}
