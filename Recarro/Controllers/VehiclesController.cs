using Microsoft.AspNetCore.Mvc;
using Recarro.Data;
using Recarro.Data.Models;
using Recarro.Models.Vehicles;
using System.Collections.Generic;
using System.Linq;

namespace Recarro.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly RecarroDbContext data;

        public VehiclesController(RecarroDbContext data)
            => this.data = data;

        public IActionResult Create() => View(new CreateVehicleModel
        {
            Categories = this.GetVehicleCategories(),
            EngineTypes = this.GetEngineTypes()
        });

        [HttpPost]
        public IActionResult Create(CreateVehicleModel vehicleModel)
        {
            if (!this.data.Categories.Any(c => c.Id == vehicleModel.CategoryId))
            {
                this.ModelState.AddModelError(nameof(vehicleModel.CategoryId), "Category does not exist!");
            }

            if (!this.data.EngineTypes.Any(et => et.Id == vehicleModel.EngineTypeId))
            {
                this.ModelState.AddModelError(nameof(vehicleModel.EngineTypeId), "Engine type does not exist!");
            }

            if (!ModelState.IsValid)
            {
                vehicleModel.Categories = this.GetVehicleCategories();
                vehicleModel.EngineTypes = this.GetEngineTypes();

                return View(vehicleModel);
            }

            var vehicle = new Vehicle
            {
                Make = vehicleModel.Make,
                Model = vehicleModel.Model,
                Year = vehicleModel.Year,
                ImageURL = vehicleModel.ImageURL,
                Description = vehicleModel.Description,
                PricePerDay = vehicleModel.PricePerDay,
                CategoryId = vehicleModel.CategoryId,
                EngineTypeId = vehicleModel.EngineTypeId,
                IsAvailable = true
            };

            this.data.Vehicles.Add(vehicle);
            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult List()
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
                    .ToList();

            return View(vehicles);
        }

        private IEnumerable<CreateCategoryModel> GetVehicleCategories()
            => this.data
                .Categories
                .Select(c => new CreateCategoryModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();

        private IEnumerable<CreateEngineTypeModel> GetEngineTypes()
            => this.data
                .EngineTypes
                .Select(c => new CreateEngineTypeModel
                {
                    Id = c.Id,
                    Type = c.Type
                })
                .ToList();
    }   
}
