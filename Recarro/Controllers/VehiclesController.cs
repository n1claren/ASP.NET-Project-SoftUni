using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recarro.Infrastructure;
using Recarro.Services.Vehicles;
using Recarro.Models.Vehicles;
using Recarro.Services.Users;
using Recarro.Models.Rent;
using System;

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

            var vehicles = vService.VehiclesByUserId(userId);

            return View(vehicles);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.GetId();
            var renterId = uService.GetRenterId(userId);
            var vehicle = this.vService.VehicleDetails(id);

            if (renterId == 0 && !User.isAdmin())
            {
                return Unauthorized();
            }

            if (vehicle.RenterId != renterId && !User.isAdmin())
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

            if (renterId == 0 && !User.isAdmin())
            {
                return Unauthorized();
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

        public IActionResult Delete(int id)
        {
            var userId = User.GetId();
            var userIsAdmin = User.isAdmin();
            var vehicle = vService.VehicleDetails(id);
            var renterId = uService.GetRenterId(userId);

            if (renterId == 0 && !userIsAdmin)
            {
                return BadRequest();
            }

            return View(vehicle);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(VehicleServiceFullModel vehicle)
        {
            var deleted = this.vService.DeleteVehicle(vehicle.Id);

            if (!deleted)
            {
                return BadRequest();
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var vehicle = vService.VehicleDetails(id);

            return View(vehicle);
        }

        [Authorize]
        public IActionResult Rent(int id)
        {
            ViewBag.VehicleId = id;

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Rent(CreateRentFormModel rent)
        {
            if (!ModelState.IsValid)
            {
                return View(rent);
            }

            if ((rent.EndDate - rent.StartDate).TotalDays < 0)
            {
                return View(rent);
            }

            this.vService.RentVehicle(rent.StartDate, rent.EndDate, rent.UserId, rent.VehicleId);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Free(int id)
        {
            var userId = this.User.GetId();
            var renterId = this.uService.GetRenterId(userId);
            var userIsAdmin = this.User.isAdmin();
            var userIsRenter = this.uService.UserIsRenter(userId);

            if (!userIsAdmin)
            {
                if (!this.uService.VehicleRentedByOrTo(userId, renterId, id))
                {
                    return Unauthorized();
                }
            }

            this.vService.FreeVehicle(id);

            if (userIsAdmin)
            {
                return RedirectToAction("List", "Vehicles", new { area = "Admin" });
            }
            else if (userIsRenter)
            {
                return RedirectToAction("RenterVehicles", "Vehicles");
            }
            else
            {
                return RedirectToAction("Rents", "Vehicles");
            }
        }

        public IActionResult Rents()
        {
            var userId = this.User.GetId();
            ViewBag.UserId = userId;

            var rentsByUser = this.uService.UserRents(userId);

            return View(rentsByUser);
        }

        public IActionResult Rented()
        {
            var vehicles = this.vService.RentedVehicles();

            return View(vehicles);
        }
    }
}
