using Microsoft.AspNetCore.Mvc;
using Recarro.Data;
using Recarro.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace Recarro.Controllers
{
    [ApiController]
    [Route("api/vehicles")]
    public class VehiclesApiController : ControllerBase
    {
        private readonly RecarroDbContext data;

        public VehiclesApiController(RecarroDbContext data)
            => this.data = data;

        [HttpGet]
        public ActionResult<List<Vehicle>> GetAllVehiclesData()
        {
            var vehicles = this.data.Vehicles.ToList();

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
            var vehicle = this.data.Vehicles.FirstOrDefault(x => x.Id == id);

            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }
    }
}
