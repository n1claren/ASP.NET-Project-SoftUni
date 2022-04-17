using Microsoft.EntityFrameworkCore;
using Recarro.Data;
using System;

namespace Recarro.Tests.Data
{
    public class DatabaseMock
    {
        public static RecarroDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<RecarroDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                return new RecarroDbContext(dbContextOptions);
            }
        }
    }
}
