using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recarro.Infrastructure;
using Recarro.Services.Vehicles;
using Recarro.Models.Vehicles;
using Recarro.Services.Users;

namespace Recarro.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly IVehicleService vService;
        private readonly IUserService uService;

        public VehiclesController(IVehicleService vService, IUserService uService)
        {
            this.vService = vService;
            this.uService = uService;
        }

        [Authorize]
        public IActionResult Create()
        {
            var userId = this.User.GetId();
            var userIsRenter = this.uService.UserIsRenter(userId);

            if (!userIsRenter)
            {
                return RedirectToAction("Create", "Renters");
            }

            return View(new CreateVehicleModel
            {
                Categories = this.vService.GetVehicleCategories(),
                EngineTypes = this.vService.GetEngineTypes()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(CreateVehicleModel vehicleModel)
        {
            var userId = this.User.GetId();
            var renterId = uService.GetRenterId(userId);

            if (renterId == 0)
            {
                return RedirectToAction("Create", "Renters");
            }

            if (!this.vService.CategoryExists(vehicleModel.CategoryId))
            {
                this.ModelState.AddModelError(nameof(vehicleModel.CategoryId), "Category does not exist!");
            }

            if (!this.vService.EngineTypeExists(vehicleModel.EngineTypeId))
            {
                this.ModelState.AddModelError(nameof(vehicleModel.EngineTypeId), "Engine type does not exist!");
            }

            if (!ModelState.IsValid)
            {
                vehicleModel.Categories = this.vService.GetVehicleCategories();
                vehicleModel.EngineTypes = this.vService.GetEngineTypes();

                return View(vehicleModel);
            }

            this.vService.CreateVehicle(vehicleModel.Make,
                                        vehicleModel.Model,
                                        vehicleModel.Year,
                                        vehicleModel.ImageURL,
                                        vehicleModel.Description,
                                        vehicleModel.PricePerDay,
                                        vehicleModel.CategoryId,
                                        vehicleModel.EngineTypeId,
                                        renterId);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult List([FromQuery] VehicleQueryServiceModel query)
        {
            query = this.vService.List(query);

            return View(query);
        }

        [Authorize]
        public IActionResult RenterVehicles()
        {
            var userId = this.User.GetId();
            var userIsRenter = this.uService.UserIsRenter(userId);

            if (!userIsRenter)
            {
                return BadRequest();
            }

            var vehicles = vService.VehiclesById(userId);

            return View(vehicles);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.GetId();
            var renterId = uService.GetRenterId(userId);
            var vehicle = this.vService.VehicleDetails(id);

            if (renterId == 0 || vehicle.RenterId != renterId)
            {
                return Unauthorized();
            }

            return View(new CreateVehicleModel
            {
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                PricePerDay = vehicle.PricePerDay,
                ImageURL = vehicle.ImageURL,
                Description = vehicle.Description,
                CategoryId = vehicle.CategoryId,
                EngineTypeId = vehicle.EngineTypeId,
                Categories = this.vService.GetVehicleCategories(),
                EngineTypes = this.vService.GetEngineTypes()
            });    
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, CreateVehicleModel vehicleModel)
        {
            var userId = this.User.GetId();
            var renterId = uService.GetRenterId(userId);

            if (renterId == 0)
            {
                return BadRequest();
            }

            if (!this.vService.CategoryExists(vehicleModel.CategoryId))
            {
                this.ModelState.AddModelError(nameof(vehicleModel.CategoryId), "Category does not exist!");
            }

            if (!this.vService.EngineTypeExists(vehicleModel.EngineTypeId))
            {
                this.ModelState.AddModelError(nameof(vehicleModel.EngineTypeId), "Engine type does not exist!");
            }

            if (!ModelState.IsValid)
            {
                vehicleModel.Categories = this.vService.GetVehicleCategories();
                vehicleModel.EngineTypes = this.vService.GetEngineTypes();

                return View(vehicleModel);
            }

            var edited = this.vService.EditVehicle(id,
                                      vehicleModel.Make,
                                      vehicleModel.Model,
                                      vehicleModel.Year,
                                      vehicleModel.ImageURL,
                                      vehicleModel.Description,
                                      vehicleModel.PricePerDay,
                                      vehicleModel.CategoryId,
                                      vehicleModel.EngineTypeId,
                                      renterId);

            if (!edited)
            {
                return BadRequest();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
