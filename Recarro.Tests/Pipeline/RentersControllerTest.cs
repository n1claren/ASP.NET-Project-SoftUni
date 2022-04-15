using MyTested.AspNetCore.Mvc;
using Recarro.Controllers;
using Recarro.Data.Models;
using Recarro.Models.Renters;
using System.Linq;
using Xunit;

namespace Recarro.Tests.Pipeline
{
    public class RentersControllerTest
    {
        [Fact]
        public void GetCreateShouldAuthorizeUsersAndReturnView()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Renters/Create")
                    .WithUser())
                .To<RentersController>(c => c.Create())
                .Which()
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();

        [Theory]
        [InlineData("Renter", "+359123456789")]
        public void PostCreateShouldAuthorizeUsersAndReturnRedirectWithValidModel(string renterName, string phoneNumber)
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Renters/Create")
                    .WithMethod(HttpMethod.Post)
                    .WithFormFields(new
                    {
                        Name = renterName,
                        PhoneNumber = phoneNumber
                    })
                    .WithUser()
                    .WithAntiForgeryToken())
                .To<RentersController>(c => c.Create(new BecomeRenterFormModel
                {
                    Name = renterName,
                    PhoneNumber = phoneNumber
                }))
                .Which()
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
