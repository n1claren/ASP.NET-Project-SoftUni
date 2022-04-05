using Microsoft.AspNetCore.Mvc;
using Recarro.Services.Vehicles;
using Recarro.Models;
using Recarro.Models.Home;
using Recarro.Models.Vehicles;
using System.Diagnostics;
using System.Linq;

namespace Recarro.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVehicleService vService;

        public HomeController(IVehicleService vService) 
            => this.vService = vService;

        public IActionResult Index()
        {
            var vehicles = vService
                .LastThreeAddedVehicles()
                .Select(v => new ListingViewModel
                {
                    Id = v.Id,
                    Make = v.Make,
                    Model = v.Model,
                    Year = v.Year,
                    ImageURL = v.ImageURL
                })
                .ToList();

            var vehiclesLeft = vService.GetAllVehicles().Count() - 3;

            return View(new HomeViewModel
            {
                Vehicles = vehicles,
                VehiclesLeft = vehiclesLeft
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
