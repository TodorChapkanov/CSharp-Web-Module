using Panda.Domain;
using System.Collections.Generic;

namespace Panda.Services
{
    public interface IUserServices
    {
        PandaUser GetUserByUsername(string username);

        ICollection<string> GetAllUsernames();
    }
}
