using Recarro.Data.Models;
using Recarro.Services.Vehicles;
using Recarro.Tests.Data;
using System.Linq;
using Xunit;

namespace Recarro.Tests.Services
{
    public class VehicleServiceTest
    {
        [Fact]
        public void CategoryExistsReturnsCorrectResult()
        {
            using var data = DatabaseMock.Instance;

            data.Categories.Add(new Category());
            data.Categories.Add(new Category());
            data.SaveChanges();

            var vehicleService = new VehicleService(data);

            Assert.True(vehicleService.CategoryExists(1));
            Assert.True(vehicleService.CategoryExists(2));
            Assert.False(vehicleService.CategoryExists(3));
        }

        [Fact]
        public void CreateVehicleModelBindingWorksCorrectly()
        {
            using var data = DatabaseMock.Instance;
            var vehicleService = new VehicleService(data);

            vehicleService.CreateVehicle("Nissan", "GT-R", 2021, "no-image", "no-description", 500, 1, 1, 1);

            var vehicle = data.Vehicles.Find(1);

            Assert.Equal("Nissan", vehicle.Make);
            Assert.Equal("GT-R", vehicle.Model);
            Assert.Equal(2021, vehicle.Year);
            Assert.Equal(500, vehicle.PricePerDay);
            Assert.Equal("no-image", vehicle.ImageURL);
            Assert.Equal("no-description", vehicle.Description);
        }

        [Fact]
        public void EditVehicleModelBindingWorksCorrectly()
        {
            using var data = DatabaseMock.Instance;

            data.Vehicles.Add(new Vehicle());
            data.SaveChanges();

            var vehicleService = new VehicleService(data);

            vehicleService.EditVehicle(1, "Nissan", "GT-R", 2021, "no-image", "no-description", 500, 1, 1, 1);

            var vehicle = data.Vehicles.Find(1);

            Assert.Equal("Nissan", vehicle.Make);
            Assert.Equal("GT-R", vehicle.Model);
            Assert.Equal(2021, vehicle.Year);
            Assert.Equal(500, vehicle.PricePerDay);
            Assert.Equal("no-image", vehicle.ImageURL);
            Assert.Equal("no-description", vehicle.Description);
        }

        [Fact]
        public void EngineTypeExistsReturnsCorrectResult()
        {
            using var data = DatabaseMock.Instance;

            data.EngineTypes.Add(new EngineType());
            data.EngineTypes.Add(new EngineType());
            data.SaveChanges();

            var vehicleService = new VehicleService(data);

            Assert.True(vehicleService.EngineTypeExists(1));
            Assert.True(vehicleService.EngineTypeExists(2));
            Assert.False(vehicleService.EngineTypeExists(3));
        }

        [Fact]
        public void GetAllVehiclesShouldReturnAllVehicles()
        {
            using var data = DatabaseMock.Instance;

            data.Vehicles.AddRange(Vehicles.TenVehicles);
            data.SaveChanges();

            var vehicleService = new VehicleService(data);

            Assert.Equal(10, vehicleService.GetAllVehicles().Count());
        }

        [Fact]
        public void GetVehicleByIdShouldReturnCorrectVehicle()
        {
            using var data = DatabaseMock.Instance;

            data.Vehicles.AddRange(Vehicles.TenVehicles);

            var vehicleAdd = new Vehicle();
            vehicleAdd.Make = "Nissan";
            vehicleAdd.Model = "GT-R";

            data.Vehicles.Add(vehicleAdd);
            
            data.SaveChanges();

            var vehicleService = new VehicleService(data);

            var vehicle = vehicleService.GetVehicleById(11);

            Assert.Equal("Nissan", vehicle.Make);
            Assert.Equal("GT-R", vehicle.Model);
        }

        [Fact]
        public void LastThreeAddedVehiclesShouldReturnThreeServiceModels()
        {
            using var data = DatabaseMock.Instance;

            data.Vehicles.AddRange(Vehicles.TenVehicles);
            data.SaveChanges();

            var vehicleService = new VehicleService(data);

            var vehicles = vehicleService.LastThreeAddedVehicles();

            Assert.Equal(3, vehicles.Count());
            Assert.Equal(new VehicleServiceModel().GetType().ToString(), vehicles.First().GetType().ToString());
        }

        [Fact]
        public void DeleteVehicleDeletesIt()
        {
            using var data = DatabaseMock.Instance;

            data.Vehicles.AddRange(Vehicles.TenVehicles);
            data.SaveChanges();

            var vehicleService = new VehicleService(data);

            vehicleService.DeleteVehicle(1);
            vehicleService.DeleteVehicle(2);

            Assert.Equal(8, data.Vehicles.Count());
        }

        [Fact]
        public void FreeVehicleSetsParameterCorrectly()
        {
            var userId = "TestUser";

            using var data = DatabaseMock.Instance;

            var vehicle = new Vehicle();
            vehicle.CurrentUser = userId;

            data.Vehicles.Add(vehicle);
            data.SaveChanges();

            var vehicleService = new VehicleService(data);

            vehicleService.FreeVehicle(1);

            Assert.Null(vehicle.CurrentUser);
        }
    }
}
