using Panda.Domain;
using System.Linq;

namespace Panda.Services
{
    public interface IPackageServices
    {
        int Create(string description, decimal weight, string shipingAddress, string recipient);

        IQueryable<Package> GetAllPackagesAsQueryable();

        Package GetPackageById(string id);

        void Acquire(string id);

        void ShipPackage(string id);

        void Deliver(string id);
    }
}
