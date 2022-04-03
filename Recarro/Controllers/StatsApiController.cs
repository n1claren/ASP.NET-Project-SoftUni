using Microsoft.AspNetCore.Mvc;
using Recarro.Data;
using Recarro.Models.Api;
using System.Linq;

namespace Recarro.Controllers
{
    [ApiController]
    [Route("api/stats")]
    public class StatsApiController : ControllerBase
    {
        private readonly RecarroDbContext data;

        public StatsApiController(RecarroDbContext data) 
            => this.data = data;

        [HttpGet]
        public StatsResponseModel GetStats()
        {
            var stats = new StatsResponseModel
            {
                TotalVehicles = this.data.Vehicles.Count(),
                TotalRents = 0, // table yet to be made
                TotalUsers = this.data.Users.Count()
            };

            return stats;
        }
    }
}
