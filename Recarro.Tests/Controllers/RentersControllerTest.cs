using MyTested.AspNetCore.Mvc;
using Recarro.Controllers;
using Recarro.Data.Models;
using Recarro.Models.Renters;
using System.Linq;
using Xunit;

namespace Recarro.Tests.Controllers
{
    public class RentersControllerTest
    {
        [Fact]
        public void GetCreateShouldBeForAuthorizedUsersAndReturnView()
            => MyController<RentersController>
                .Instance()
                .Calling(c => c.Create())
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData("Renter", "+359123456789")]
        public void PostCreateShouldAuthorizeUsersAndReturnRedirectWithValidModel(string renterName, string phoneNumber)
            => MyController<RentersController>
                .Instance(controller => controller
                    .WithUser())
                .Calling(c => c.Create(new BecomeRenterFormModel
                {
                    Name = renterName,
                    PhoneNumber = phoneNumber
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
                    .WithSet<Renter>(r => r
                        .Any(r =>
                            r.Name == renterName &&
                            r.PhoneNumber == phoneNumber &&
                            r.UserId == TestUser.Identifier)))
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<HomeController>(c => c.Index()));
    }
}
