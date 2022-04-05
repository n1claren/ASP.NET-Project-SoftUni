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
    }
}
