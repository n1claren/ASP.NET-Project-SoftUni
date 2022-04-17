using Recarro.Data.Models;
using Recarro.Services.Stats;
using Recarro.Tests.Data;
using Xunit;

namespace Recarro.Tests.Services
{
    public class StatsServiceTest
    {
        [Fact]
        public void UserIsRenterShouldReturnTrueWhenUserIsRenter()
        {
            using var data = DatabaseMock.Instance;

            data.Vehicles.AddRange(Vehicles.TenVehicles);
            data.Rents.AddRange(Rents.EightRents);
            data.Users.AddRange(Users.FiveUsers);
            data.SaveChanges();

            var statsService = new StatsService(data);

            var stats = statsService.GetStats();

            var users = stats.TotalUsers;
            var vehicles = stats.TotalVehicles;
            var rents = stats.TotalRents;

            Assert.Equal(10, vehicles);
            Assert.Equal(8, rents);
            Assert.Equal(5, users);
        }
    }
}
