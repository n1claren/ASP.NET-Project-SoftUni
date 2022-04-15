using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recarro.Data;
using Recarro.Data.Models;
using Recarro.Infrastructure;
using Recarro.Models.Renters;
using System.Linq;

namespace Recarro.Controllers
{
    public class RentersController : Controller
    {
        private readonly RecarroDbContext data;

        public RentersController(RecarroDbContext data) 
            => this.data = data;

        [Authorize]
        public IActionResult Create() => View();

        [Authorize]
        [HttpPost]
        public IActionResult Create(BecomeRenterFormModel renterModel)
        {
            var userId = this.User.GetId();
            var userIsRenter = this.data.Renters.Any(r => r.UserId == userId);

            if (userIsRenter)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(renterModel);
            }

            var renter = new Renter
            {
                Name = renterModel.Name,
                PhoneNumber = renterModel.PhoneNumber,
                UserId = userId
            };

            this.data.Renters.Add(renter);
            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
