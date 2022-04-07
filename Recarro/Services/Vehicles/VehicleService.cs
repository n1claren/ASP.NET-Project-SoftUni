using Recarro.Data;
using Recarro.Data.Models;
using Recarro.Models.Vehicles;
using System.Collections.Generic;
using System.Linq;

namespace Recarro.Services.Vehicles
{
    public class VehicleService : IVehicleService
    {
        private readonly RecarroDbContext data;

        public VehicleService(RecarroDbContext data) 
            => this.data = data;

        public bool CategoryExists(int categoryId)
            => this.data.Categories.Any(c => c.Id == categoryId);

        public void CreateVehicle(string make, 
                                  string model, 
                                  int year, 
                                  string imageUrl, 
                                  string description, 
                                  decimal pricePerDay,  
                                  int categoryId, 
                                  int engineTypeId, 
                                  int renterId)
        {
            var vehicle = new Vehicle
            {
                Make = make,
                Model = model,
                Year = year,
                ImageURL = imageUrl,
                Description = description,
                PricePerDay = pricePerDay,
                IsAvailable = true,
                CategoryId = categoryId,
                EngineTypeId = engineTypeId,
                RenterId = renterId
            };

            this.data.Vehicles.Add(vehicle);
            this.data.SaveChanges();
        }

        public bool EditVehicle(int id,
                                string make,
                                string model,
                                int year,
                                string imageUrl,
                                string description,
                                decimal pricePerDay,
                                int categoryId,
                                int engineTypeId,
                                int renterId)
        {
            var vehicle = this.data.Vehicles.Find(id);

            if (vehicle == null) // || vehicle.RenterId != renterId
            {
                return false;
            }

            vehicle.Make = make;
            vehicle.Model = model;
            vehicle.Year = year;
            vehicle.ImageURL = imageUrl;
            vehicle.Description = description;
            vehicle.PricePerDay = pricePerDay;
            vehicle.CategoryId = categoryId;
            vehicle.EngineTypeId = engineTypeId;

            this.data.SaveChanges();

            return true;
        }

        public bool EngineTypeExists(int engineTypeId)
            => this.data.EngineTypes.Any(et => et.Id == engineTypeId);

        public IEnumerable<Vehicle> GetAllVehicles()
            => this.data.Vehicles.ToList();

        public IEnumerable<CreateEngineTypeModel> GetEngineTypes()
            => this.data
                   .EngineTypes
                   .Select(c => new CreateEngineTypeModel
                   {
                       Id = c.Id,
                       Type = c.Type
                   })
                   .ToList();

        public Vehicle GetVehicleById(int id)
            => this.data.Vehicles.FirstOrDefault(x => x.Id == id);

        public IEnumerable<CreateCategoryModel> GetVehicleCategories()
            => this.data
                   .Categories
                   .Select(c => new CreateCategoryModel
                   {
                       Id = c.Id,
                       Name = c.Name
                   })
                   .ToList();

        public IEnumerable<VehicleServiceModel> LastThreeAddedVehicles()
            => this.data
                 .Vehicles
                 .Where(v => v.IsAvailable == true)
                 .OrderByDescending(v => v.Id)
                 .Select(v => new VehicleServiceModel
                 {
                     Id = v.Id,
                     Make = v.Make,
                     Model = v.Model,
                     Year = v.Year,
                     ImageURL = v.ImageURL
                 })
                 .Take(3)
                 .ToList();

        public VehicleQueryServiceModel List(VehicleQueryServiceModel query)
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
                .Skip((query.CurrentPage - 1) * query.VehiclesPerPage)
                .Take(query.VehiclesPerPage)
                .Where(v => v.IsAvailable == true)
                .Select(v => new VehicleServiceModel
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

            return query;
        }

        public VehicleServiceFullModel VehicleDetails(int id)
            => this.data
                .Vehicles
                .Where(v => v.Id == id)
                .Select(v => new VehicleServiceFullModel
                {
                    Id = v.Id,
                    Make = v.Make,
                    Model = v.Model,
                    Year = v.Year,
                    ImageURL = v.ImageURL,
                    Description = v.Description,
                    PricePerDay = v.PricePerDay,
                    CategoryId = v.CategoryId,
                    CategoryName = v.Category.Name,
                    EngineTypeId = v.EngineTypeId,
                    EngineTypeName = v.EngineType.Type,
                    RenterId = v.RenterId
                })
                .FirstOrDefault();

        public IEnumerable<VehicleServiceModel> VehiclesById(string id)
        {
            var renterId = this.data
                .Renters
                .Where(r => r.UserId == id)
                .Select(r => r.Id)
                .FirstOrDefault();

            var vehicles = this.data
                .Vehicles
                .Where(v => v.RenterId == renterId)
                .Select(v => new VehicleServiceModel
                {
                    Id = v.Id,
                    Make = v.Make,
                    Model = v.Model,
                    Year = v.Year,
                    ImageURL = v.ImageURL
                })
                .ToList();

            return vehicles;
        }
    }
}
