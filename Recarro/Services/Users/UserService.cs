using Recarro.Data;
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

        public bool VehicleBelongsToRenter(int renterId, int vehicleId)
        {
            var vehicle = this.data.Vehicles.Where(v => v.Id == vehicleId).FirstOrDefault();
            var renter = this.data.Renters.Where(v => v.Id == renterId).FirstOrDefault();

            if (vehicle == null || renter == null)
            {
                return false;
            }

            if (vehicle.RenterId == renter.Id)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}