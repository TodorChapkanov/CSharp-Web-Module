using Panda.Data;
using Panda.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Panda.Services
{
    public class UserServices : IUserServices
    {
        private readonly PandaDbContex contex;

        public UserServices(PandaDbContex contex)
        {
            this.contex = contex;
        }

        public ICollection<string> GetAllUsernames()
        {
            var usernames = this.contex.Users.Select(user => user.UserName).ToList();

            return usernames;
        }

        public PandaUser GetUserByUsername(string username)
        {
            var userFromDb = this.contex.Users.SingleOrDefault(user => user.UserName == username);

            return userFromDb;
        }
    }
}
