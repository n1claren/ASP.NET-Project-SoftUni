using Recarro.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace Recarro.Tests.Data
{
    public static class Vehicles
    {
        public static IEnumerable<Vehicle> TenVehicles
            => Enumerable.Range(0, 10).Select(i => new Vehicle());
    }
}
