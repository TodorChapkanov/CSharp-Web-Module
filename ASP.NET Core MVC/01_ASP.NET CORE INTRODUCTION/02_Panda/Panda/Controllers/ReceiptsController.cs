using Microsoft.AspNetCore.Mvc;
using Panda.Services;
using Panda.ViewModels.Receipts;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Panda.Controllers
{
    public class ReceiptsController : Controller
    {
        private readonly IReceiptServices receiptServices;

        public ReceiptsController(IReceiptServices receiptServices)
        {
            this.receiptServices = receiptServices;
        }

        [Authorize]
        public IActionResult Index()
        {
            var viewModel = this.receiptServices.GetAllReceipts().Select(receipt => new ReceiptsIndexViewModel
            {
                Fee = receipt.Fee,
                Id = receipt.Id,
                IssuedOn = receipt.IssuedOn.ToString("dd/MM/yyyy"),
                Recipient = receipt.Recipient.UserName
            }).ToList();

            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult Details(string id)
        {
            var receiptFromDb = this.receiptServices.GetReceiptById(id);

            var viewModel = new ReceiptDetailsViewModel
            {
                ReceiptNumber = receiptFromDb.Id,
                IssuedOn = receiptFromDb.IssuedOn.ToString("dd/MM/yyyy"),
                Fee = receiptFromDb.Fee,
                DeliveryAddress = receiptFromDb.Package.ShippingAddress,
                PackageDescription = receiptFromDb.Package.Description,
                PackageWeight = receiptFromDb.Package.Weight,
                RecipientUserName = receiptFromDb.Recipient.UserName
            };

            return this.View(viewModel);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Create(string id)
        {
            this.receiptServices.Create(id);

            return this.Redirect("/");
        }
    }
}
