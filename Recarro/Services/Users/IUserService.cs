using System.Collections.Generic;

namespace Recarro.Services.Users
{
    public interface IUserService
    {
        public bool UserIsRenter(string userId);

        public int GetRenterId(string userId);

        public bool VehicleRentedByOrTo(string userId, int renterId, int vehicleId);

        public IEnumerable<UserRentedVehiclesModel> UserRents(string userId);
    }
}