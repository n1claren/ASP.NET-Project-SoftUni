using MyTested.AspNetCore.Mvc;
using Recarro.Controllers;
using Recarro.Models.Home;
using Xunit;

using Recarro.Tests.Data;

namespace Recarro.Tests.Controllers
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnViewWithModel() 
            => MyController<HomeController>
                .Instance(controller => controller
                    .WithData(Vehicles.TenVehicles))
                .Calling(c => c.Index())
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<HomeViewModel>()
                    .Passing(m => m.Vehicles.Count == 3));

        [Fact]
        public void ErrorShouldReturnView()
            => MyController<HomeController>
                .Instance()
                .Calling(c => c.Error())
                .ShouldReturn()
                .View();
    }
}
