using Recarro.Data.Models;
using Recarro.Services.Users;
using Recarro.Tests.Data;
using System.Linq;
using Xunit;

namespace Recarro.Tests.Services
{
    public class UserServiceTest
    {
        [Fact]
        public void UserIsRenterShouldReturnTrueWhenUserIsRenter()
        {
            const string userId = "TestUser";

            using var data = DatabaseMock.Instance;

            data.Renters.Add(new Renter { UserId = userId });
            data.SaveChanges();

            var userService = new UserService(data);

            var result = userService.UserIsRenter(userId);

            Assert.True(result);
        }

        [Fact]
        public void UserIsRenterShouldReturnFalseWhenUserIsNotRenter()
        {
            const string userId = "TestUser";
            const string anotherUserId = "AnotherUser";

            using var data = DatabaseMock.Instance;

            data.Renters.Add(new Renter { UserId = userId });
            data.SaveChanges();

            var userService = new UserService(data);

            var result = userService.UserIsRenter(anotherUserId);

            Assert.False(result);
        }

        [Fact]
        public void GetRenterVehiclesShouldGetOnlyRenterVehicles()
        {
            const string userId = "TestUser";
            const string anotherUserId = "AnotherUser";

            using var data = DatabaseMock.Instance;

            data.Rents.Add(new Rent { UserId = userId, Vehicle = new Vehicle { CurrentUser = userId } });
            data.Rents.Add(new Rent { UserId = anotherUserId, Vehicle = new Vehicle { CurrentUser = anotherUserId } });
            data.SaveChanges();

            var userService = new UserService(data);

            var userVehicles = userService.UserRents(userId);

            var result = userVehicles.Count();

            Assert.Equal(1, result);
        }

        [Fact]
        public void GetRenterIdShouldReturnCorrectRenterId()
        {
            const string userId = "TestUser";
            const string anotherUserId = "AnotherUser";

            using var data = DatabaseMock.Instance;

            data.Renters.Add(new Renter { UserId = userId });
            data.Renters.Add(new Renter { UserId = anotherUserId });
            data.SaveChanges();

            var userService = new UserService(data);

            var result = userService.GetRenterId(userId);
            var anotherResult = userService.GetRenterId(anotherUserId);

            Assert.Equal(1, result);
            Assert.Equal(2, anotherResult);
        }
    }
}
