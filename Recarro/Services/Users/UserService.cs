using Recarro.Data;
using System.Collections.Generic;
using System.Linq;

namespace Recarro.Services.Users
{
    public class UserService : IUserService
    {
        private readonly RecarroDbContext data;

        public UserService(RecarroDbContext data)
            => this.data = data;

        public int GetRenterId(string userId)
            => this.data
                .Renters
                .Where(r => r.UserId == userId)
                .Select(r => r.Id)
                .FirstOrDefault();

        public bool UserIsRenter(string userId)
            => this.data.Renters.Any(r => r.UserId == userId);

        public bool VehicleRentedByOrTo(string userId, int renterId, int vehicleId)
        {
            var vehicle = this.data.Vehicles.Where(v => v.Id == vehicleId).FirstOrDefault();

            if (vehicle == null)
            {
                return false;
            }

            if (vehicle.RenterId == renterId || vehicle.CurrentUser == userId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<UserRentedVehiclesModel> UserRents(string userId)
            => this.data
                .Rents
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.Id)
                .Select(r => new UserRentedVehiclesModel
                {
                    Id = r.Vehicle.Id,
                    Make = r.Vehicle.Make,
                    Model = r.Vehicle.Model,
                    Year = r.Vehicle.Year,
                    RentedFrom = r.StartDate,
                    RentedUntil = r.EndDate,
                    CurrentUser = r.Vehicle.CurrentUser
                })
                .ToList();
    }
}