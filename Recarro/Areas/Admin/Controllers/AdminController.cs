using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Recarro.Areas.Admin.Controllers
{
    [Area(AdminAreaConstants.AdminAreaName)]
    [Authorize(Roles = WebConstants.AdministratorRoleName)]
    public abstract class AdminController : Controller
    {

    }
}
