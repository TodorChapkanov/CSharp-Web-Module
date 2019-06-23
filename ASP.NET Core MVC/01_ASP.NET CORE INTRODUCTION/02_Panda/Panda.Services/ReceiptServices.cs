using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Panda.Data;
using Panda.Domain;

namespace Panda.Services
{
    public class ReceiptServices : IReceiptServices
    {
        private const decimal FeePerKilogram = 2.67M;
        private readonly PandaDbContex contex;
        private readonly IPackageServices packageServices;

        public ReceiptServices(PandaDbContex contex, IPackageServices packageServices)
        {
            this.contex = contex;
            this.packageServices = packageServices;
        }

        public IQueryable<Receipt> GetAllReceipts()
        {
            var receipts = this.contex.Receipts;

            return receipts;
        }

        public Receipt GetReceiptById(string id)
        {
            var receiptFromDb = this.contex.Receipts
                .Include(receipt => receipt.Package)
                .Include(receipt => receipt.Recipient)
                .FirstOrDefault(receipt => receipt.Id == id);

            return receiptFromDb;
        }

        public void Create(string packageId)
        {
            var packageFromDb = this.packageServices.GetPackageById(packageId);
            packageFromDb.Status = this.contex.Statuses.FirstOrDefault(status => status.Name == "Acquired");
            var fee = packageFromDb.Weight * FeePerKilogram;
            var receipt = new Receipt
            {
                Fee = fee,
                Package = packageFromDb,
                Recipient = packageFromDb.Recipient,
                IssuedOn = DateTime.UtcNow
            };


            this.contex.Receipts.Add(receipt);
            this.contex.SaveChanges();
        }
    }
}
