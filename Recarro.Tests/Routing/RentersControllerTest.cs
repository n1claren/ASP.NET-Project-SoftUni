using MyTested.AspNetCore.Mvc;
using Recarro.Controllers;
using Xunit;

namespace Recarro.Tests.Routing
{
    public class RentersControllerTest
    {
        [Fact]
        public void GetCreateShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap("/Renters/Create")
                .To<RentersController>(c => c.Create());

        [Fact]
        public void PostCreateShouldBeMapped()
            => MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Renters/Create")
                    .WithMethod(HttpMethod.Post))
                .To<RentersController>(c => c.Create());
    }
}
