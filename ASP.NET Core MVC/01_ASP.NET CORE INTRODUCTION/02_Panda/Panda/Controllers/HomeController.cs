using Microsoft.AspNetCore.Mvc;
using Panda.Services;
using Panda.ViewModels.Packages;
using System.Security.Claims;
using System.Linq;

namespace Panda.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPackageServices packageServices;

        public HomeController(IPackageServices packageServices)
        {
            this.packageServices = packageServices;
        }

        public IActionResult Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var viewModel = this.packageServices.GetAllPackagesAsQueryable().Where(package => package.RecipientId == userId).Select(package => new PackageLogedUserViewModel
            {
                Id = package.Id,
                Description = package.Description,
                Status = package.Status.Name
            }).ToList();

            return this.View(viewModel);
        }
    }
}
