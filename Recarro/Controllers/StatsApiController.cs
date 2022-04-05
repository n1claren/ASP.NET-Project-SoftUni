using Microsoft.AspNetCore.Mvc;
using Recarro.Interfaces.Stats;

namespace Recarro.Controllers
{
    [ApiController]
    [Route("api/stats")]
    public class StatsApiController : ControllerBase
    {
        private readonly StatsService stats;

        public StatsApiController(StatsService stats) 
            => this.stats = stats;

        [HttpGet]
        public StatsServiceModel GetStats()
        {
            var statistics = stats.GetStats();

            return new StatsServiceModel
            {
                TotalVehicles = statistics.TotalVehicles,
                TotalUsers = statistics.TotalUsers,
                TotalRents = statistics.TotalRents
            };
        }
    }
}
