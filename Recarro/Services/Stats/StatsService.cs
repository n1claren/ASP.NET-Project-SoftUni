using Recarro.Data;
using System.Linq;

namespace Recarro.Services.Stats
{
    public class StatsService : IStatsService
    {
        private readonly RecarroDbContext data;

        public StatsService(RecarroDbContext data) 
            => this.data = data;

        public StatsServiceModel GetStats()
        {
            var totalVehicles = this.data.Vehicles.Count();
            var totalUsers = this.data.Users.Count();
            var totalRents = this.data.Rents.Count();

            return new StatsServiceModel
            {
                TotalVehicles = totalVehicles,
                TotalUsers = totalUsers,
                TotalRents = totalRents
            };
        }
    }
}
