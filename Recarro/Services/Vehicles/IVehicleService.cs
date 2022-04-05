using Recarro.Data.Models;
using Recarro.Models.Vehicles;
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

        public VehicleQueryServiceModel List(VehicleQueryServiceModel query);
    }
}
