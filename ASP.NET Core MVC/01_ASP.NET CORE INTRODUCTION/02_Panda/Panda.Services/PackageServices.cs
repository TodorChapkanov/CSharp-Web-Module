using Microsoft.EntityFrameworkCore;
using Panda.Data;
using Panda.Domain;
using System;
using System.Linq;

namespace Panda.Services
{
    public class PackageServices : IPackageServices
    {
        private readonly PandaDbContex contex;
        private readonly IUserServices userServices;
        private readonly Random random;

        public PackageServices(PandaDbContex contex, IUserServices userServices, Random random)
        {
            this.contex = contex;
            this.userServices = userServices;
            this.random = random;
        }

        public void Acquire(string id)
        {
            var packageFromDb = this.GetPackageById(id);
            packageFromDb.Status = this.contex.Statuses.FirstOrDefault(status => status.Name == "Acquire");
          
        }

        public int Create(string description, decimal weight, string shippingAddress, string recipient)
        {
            var userFromDb = this.userServices.GetUserByUsername(recipient);
            
            var package = new Package
            {
                Description = description,
                Weight = weight,
                ShippingAddress = shippingAddress,
                Status = this.contex.Statuses.FirstOrDefault(status => status.Name == "Pending"),
                Recipient = userFromDb
            };

            this.contex.Packages.Add(package);
            return this.contex.SaveChanges();

        }

        public IQueryable<Package> GetAllPackagesAsQueryable()
        {
            var result = this.contex.Packages.AsQueryable().Include(package => package.Status).Include(package => package.Recipient);

            return result;
        }

        public Package GetPackageById(string id)
        {
            var packageFromDb = this.contex
                .Packages
                .Include(package => package.Status)
                .Include(package => package.Recipient)
                .FirstOrDefault(package => package.Id == id);

            return packageFromDb;
        }

       
        public void ShipPackage(string id)
        {
            var packageFromDb = this.contex.Packages.FirstOrDefault(package => package.Id == id);
            var daysForDelivery = this.random.Next(20, 40);
            packageFromDb.Status = this.contex.Statuses.FirstOrDefault(staus => staus.Name == "Shipped");
            packageFromDb.EstimatedDeliveryDate = DateTime.UtcNow.AddDays(daysForDelivery);

            this.contex.SaveChanges();
        }

        public void Deliver(string id)
        {
            var packageFromDb = this.contex.Packages.FirstOrDefault(package => package.Id == id);
            packageFromDb.Status = this.contex.Statuses.FirstOrDefault(staus => staus.Name == "Delivered");

            this.contex.SaveChanges();
        }
    }
}
