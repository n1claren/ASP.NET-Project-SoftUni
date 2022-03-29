using Microsoft.AspNetCore.Mvc;
using Recarro.Data;
using Recarro.Models;
using Recarro.Models.Home;
using Recarro.Models.Vehicles;
using System.Diagnostics;
using System.Linq;

namespace Recarro.Controllers
{
    public class HomeController : Controller
    {
        private readonly RecarroDbContext data;

        public HomeController(RecarroDbContext data)
             => this.data = data;

        public IActionResult Index()
        {
            var vehicles = this.data
                 .Vehicles
                 .Where(v => v.IsAvailable == true)
                 .OrderByDescending(v => v.Id)
                 .Select(v => new ListingViewModel
                 {
                     Id = v.Id,
                     Make = v.Make,
                     Model = v.Model,
                     Year = v.Year,
                     ImageURL = v.ImageURL
                 })
                 .Take(3)
                 .ToList();

            var vehiclesLeft = this.data.Vehicles.Count() - 3;

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
