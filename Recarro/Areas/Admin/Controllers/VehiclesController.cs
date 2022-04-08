using Microsoft.AspNetCore.Mvc;
using Recarro.Services.Vehicles;

namespace Recarro.Areas.Admin.Controllers
{
    public class VehiclesController : AdminController
    {
        private readonly IVehicleService vService;

        public VehiclesController(IVehicleService vService) 
            => this.vService = vService;

        public IActionResult List()
        {
            var vehicles = vService.MapList();

            return View(vehicles);
        }
    }
}
