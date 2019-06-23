using Panda.Domain;
using System.Linq;

namespace Panda.Services
{
    public interface IReceiptServices
    {
        IQueryable<Receipt> GetAllReceipts();

        Receipt GetReceiptById(string id);

        void Create(string packageId);
    }
}
