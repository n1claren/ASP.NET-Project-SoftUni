using Recarro.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace Recarro.Tests.Data
{
    public class Rents
    {
        public static IEnumerable<Rent> EightRents
            => Enumerable.Range(0, 8).Select(i => new Rent());
    }
}
