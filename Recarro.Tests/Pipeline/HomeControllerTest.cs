using MyTested.AspNetCore.Mvc;
using Recarro.Controllers;
using Recarro.Models.Home;
using Xunit;

using Recarro.Tests.Data;

namespace Recarro.Tests.Pipeline
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnViewWithCorrectModelAndData()
            => MyMvc
                .Pipeline()
                .ShouldMap("/")
                .To<HomeController>(c => c.Index())
                .Which(controller => controller
                    .WithData(Vehicles.TenVehicles))
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<HomeViewModel>()
                    .Passing(m => m.Vehicles.Count == 3));

        [Fact]
        public void ErrorShouldReturnView()
            => MyMvc
                .Pipeline()
                .ShouldMap("/Home/Error")
                .To<HomeController>(c => c.Error())
                .Which()
                .ShouldReturn()
                .View();
    }
}
