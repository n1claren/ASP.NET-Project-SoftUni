using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace Recarro.Tests.Data
{
    public class Users
    {
        public static IEnumerable<IdentityUser> FiveUsers
            => Enumerable.Range(0, 5).Select(i => new IdentityUser());
    }
}
