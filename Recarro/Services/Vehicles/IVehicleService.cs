using Recarro.Data.Models;
using Recarro.Models.Vehicles;
using System;
using System.Collections.Generic;

namespace Recarro.Services.Vehicles
{
    public interface IVehicleService
    {
        public IEnumerable<Vehicle> GetAllVehicles();

        public Vehicle GetVehicleById(int id);

        public IEnumerable<VehicleServiceModel> LastThreeAddedVehicles();

        public IEnumerable<CreateEngineTypeModel> GetEngineTypes();

        public IEnumerable<CreateCategoryModel> GetVehicleCategories();

        public bool CategoryExists(int categoryId);

        public bool EngineTypeExists(int engineTypeId);

        public void CreateVehicle(string make,
                                  string model,
                                  int year,
                                  string imageUrl,
                                  string description,
                                  decimal pricePerDay,
                                  int categoryId,
                                  int engineTypeId,
                                  int renterId);

        public bool EditVehicle(int id,
                                  string make,
                                  string model,
                                  int year,
                                  string imageUrl,
                                  string description,
                                  decimal pricePerDay,
                                  int categoryId,
                                  int engineTypeId,
                                  int renterId);

        public VehicleQueryServiceModel List(VehicleQueryServiceModel query);

        public IEnumerable<VehicleServiceModel> VehiclesByUserId(string id);

        public VehicleServiceFullModel VehicleDetails(int id);

        public IEnumerable<VehicleServiceFullModel> MapList();

        public bool DeleteVehicle(int id);

        public void RentVehicle(DateTime startDate, DateTime endDate, string userId, int vehicleId);

        public void FreeVehicle(int id);
    }
}
