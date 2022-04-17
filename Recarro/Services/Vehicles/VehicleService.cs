using Recarro.Data;
using Recarro.Data.Models;
using Recarro.Models.Vehicles;
using System;
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
                CurrentUser = null,
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
                 .Where(v => v.CurrentUser == null)
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
                VehicleSorting.MakeModels => vehicleQuery.OrderBy(v => v.Make).ThenBy(v => v.Model),
                VehicleSorting.Created or _ => vehicleQuery.OrderByDescending(v => v.Id),
            };

            var vehicles = vehicleQuery
                .Skip((query.CurrentPage - 1) * query.VehiclesPerPage)
                .Take(query.VehiclesPerPage)
                .Where(v => v.CurrentUser == null)
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
                    RenterId = v.RenterId,
                    RenterName = v.Renter.Name
                })
                .FirstOrDefault();

        public IEnumerable<VehicleServiceFullModel> MapList()
            => this.data
                .Vehicles
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
                    RenterId = v.RenterId,
                    CurrentUser = v.CurrentUser
                })
                .ToList();

        public IEnumerable<VehicleServiceModel> VehiclesByUserId(string id)
        {
            var renterId = this.data
                .Renters
                .Where(r => r.UserId == id)
                .Select(r => r.Id)
                .FirstOrDefault();

            var vehicles = this.data
                .Vehicles
                .Where(v => v.RenterId == renterId)
                .Select(v => new VehicleServiceFullModel
                {
                    Id = v.Id,
                    Make = v.Make,
                    Model = v.Model,
                    Year = v.Year,
                    ImageURL = v.ImageURL,
                    CurrentUser = v.CurrentUser
                })
                .ToList();

            return vehicles;
        }

        public bool DeleteVehicle(int id)
        {
            var vehicle = this.data.Vehicles.Where(v => v.Id == id).FirstOrDefault();

            if (vehicle == null)
            {
                return false;
            }

            this.data.Vehicles.Remove(vehicle);
            this.data.SaveChanges();

            return true;
        }

        public void RentVehicle(DateTime startDate, DateTime endDate, string userId, int vehicleId)
        {
            var vehicle = this.data.Vehicles.Where(v => v.Id == vehicleId).FirstOrDefault();
            var user = this.data.Users.Where(u => u.Id == userId).FirstOrDefault();

            var bill = ((decimal)(endDate - startDate).TotalDays + 1) * vehicle.PricePerDay;

            var rent = new Rent
            {
                StartDate = startDate,
                EndDate = endDate,
                UserId = userId,
                VehicleId = vehicleId,
                Vehicle = vehicle,
                User = user,
                Bill = bill
            };

            vehicle.CurrentUser = userId;

            this.data.Rents.Add(rent);
            this.data.SaveChanges();
        }

        public void FreeVehicle(int id)
        {
            this.data
                .Vehicles
                .Where(v => v.Id == id)
                .FirstOrDefault()
                .CurrentUser = null;

            this.data.SaveChanges();
        }

        public IEnumerable<VehicleServiceModel> RentedVehicles()
        {
            var vehicles = this.data
                             .Vehicles
                             .Where(v => v.CurrentUser != null)
                             .Select(v => new VehicleServiceModel
                             {
                                 Id = v.Id,
                                 Make = v.Make,
                                 Model = v.Model,
                                 Year = v.Year,
                                 ImageURL = v.ImageURL,
                             })
                             .ToList();

            foreach (var vehicle in vehicles)
            {
                vehicle.RentedUntil = this.data
                                        .Rents
                                        .Where(r => r.VehicleId == vehicle.Id)
                                        .OrderByDescending(r => r.Id)
                                        .Last()
                                        .EndDate
                                        .ToString("dd/MMMM/yyyy");
            }

            return vehicles;
        }
    }
}