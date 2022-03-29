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

        public IActionResult List([FromQuery]SearchQueryModel query)
        {
            var vehicleQuery = this.data.Vehicles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Make))
            {
                vehicleQuery = vehicleQuery.Where(v => v.Make == query.Make);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                vehicleQuery = vehicleQuery.Where(v => 
                (v.Make + " " + v.Model).ToLower().Contains(query.SearchTerm.ToLower()));
            }

            vehicleQuery = query.Sorting switch
            {
                VehicleSorting.Year => vehicleQuery.OrderByDescending(v => v.Year),
                VehicleSorting.MakeModels => vehicleQuery.OrderByDescending(v => v.Make).ThenBy(v => v.Model),
                VehicleSorting.Created or _ => vehicleQuery.OrderByDescending(v => v.Id),
            };

            var vehicles = vehicleQuery
                .Skip((query.CurrentPage - 1) * SearchQueryModel.VehiclesPerPage)
                .Take(SearchQueryModel.VehiclesPerPage)
                .Where(v => v.IsAvailable == true)
                .Select(v => new ListingViewModel
                {
                    Id = v.Id,
                    Make = v.Make,
                    Model = v.Model,
                    Year = v.Year,
                    ImageURL = v.ImageURL
                })
                .ToList();

            var makes = this.data
                .Vehicles
                .Select(v => v.Make)
                .Distinct()
                .ToList();

            var totalCars = vehicleQuery.Count();

            query.TotalCars = totalCars;
            query.Makes = makes;
            query.Vehicles = vehicles;

            return View(query);
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
