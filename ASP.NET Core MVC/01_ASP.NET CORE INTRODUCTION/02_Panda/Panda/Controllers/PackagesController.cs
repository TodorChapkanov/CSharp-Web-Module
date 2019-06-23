using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Panda.BindingModels.Packages;
using Panda.Services;
using Panda.ViewModels.Packages;

namespace Panda.Controllers
{
    public class PackagesController : Controller
    {
        private readonly IPackageServices packageServices;
        private readonly IUserServices userServices;

        public PackagesController(IPackageServices packageServices, IUserServices userServices)
        {
            this.packageServices = packageServices;
            this.userServices = userServices;
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            var usernames = this.userServices.GetAllUsernames();
            this.TempData["Users"] = usernames;
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(PackageCreateBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.Redirect("/Packages/Create");
            }

            this.packageServices.Create(model.Description, model.Weight, model.ShippingAddress, model.Recipient);
            return this.Redirect("/");
        }

        [Authorize]
        public IActionResult Details(string id)
        {
            
            var package = this.packageServices.GetPackageById(id);

            if (package == null)
            {
                return this.Redirect("/");
            }

            var viewModel = new PackageDetailsViewModel
            {
                Id = package.Id,
                Description = package.Description,
                Status = package.Status.Name,
                EstimatedDeliveryDate = package.EstimatedDeliveryDate == null ? "N/Y" : (package.Status.Name == "Acquired" ||package.Status.Name == "Delivered") ? "Delivered" : package.EstimatedDeliveryDate.Value.ToString("dd/MM/yyyy"),
                RecipientUsername = package.Recipient.UserName,
                ShippingAddress = package.ShippingAddress,
                Weight = package.Weight
            };

            return this.View(viewModel);
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Acquire(string id)
        {
            this.packageServices.Acquire(id);

            return this.Redirect($"/Receipts/Create?id={id}");
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Pending()
        {

            var viewModel = this.packageServices.
                GetAllPackagesAsQueryable()
                .Where(package => package.Status.Name == "Pending")
                .Select(package => new PackagesPendingViewModel
            {
                Id = package.Id,
                Description = package.Description,
                ShippingAdress = package.ShippingAddress,
                Weight = package.Weight,
                RecipientUsername = package.Recipient.UserName
            }).ToList();

            return this.View(viewModel);
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Ship(string id)
        {
            this.packageServices.ShipPackage(id);

            return this.Redirect("/");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Shipped()
        {
            var viewModel = this.packageServices
                .GetAllPackagesAsQueryable()
                .Where(package => package.Status.Name == "Shipped")
                .Select(package => new PackagesShippedViewModel
            {
                    Id = package.Id,
                    Description = package.Description,
                    EstimateDeliveryDate = package.EstimatedDeliveryDate != null ? package.EstimatedDeliveryDate.Value.ToString("dd/MM/yyyy"):null,
                    Weight = package.Weight,
                    RecipientUsername = package.Recipient.UserName
            }).ToList();
            return this.View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Deliver(string id)
        {
            this.packageServices.Deliver(id);
            return this.Redirect("/Packages/Shipped");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delivered()
        {
            var packagesFromDb = this.packageServices
                .GetAllPackagesAsQueryable()
                .Where(package => package.Status.Name == "Delivered" ||
                package.Status.Name == "Acquired");

            var viewModel = packagesFromDb
                .Select(package => new PackagesDeliveredAndAcqiredViewModel
                {
                    Id = package.Id,
                    Descriprion = package.Description,
                    ShippingAddress = package.ShippingAddress,
                    Weight = package.Weight,
                    RecipientUsername = package.Recipient.UserName
                }).ToList();
            return this.View(viewModel);
        }
    }
}