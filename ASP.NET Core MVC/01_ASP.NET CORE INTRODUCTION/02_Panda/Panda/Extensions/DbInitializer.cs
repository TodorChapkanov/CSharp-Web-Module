using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Panda.Data;
using Panda.Domain;
using System.Linq;

namespace Panda.Extensions
{
    public static class DbInitializer 
    {
        public static void Seed(PandaDbContex context)
        {
            if (!context.Roles.Any())
            {
                context.Roles.Add(new PandaUserRole { Name = "Admin", NormalizedName = "ADMIN" });
                context.Roles.Add(new PandaUserRole { Name = "User", NormalizedName = "USER" });
            }

            if (!context.Statuses.Any())
            {
                context.Statuses.Add(new PackageStatus { Name = "Pending" });
                context.Statuses.Add(new PackageStatus { Name = "Shipped" });
                context.Statuses.Add(new PackageStatus { Name = "Delivered" });
                context.Statuses.Add(new PackageStatus { Name = "Acquired" });
            }

            context.SaveChanges();
        }
    }
}
