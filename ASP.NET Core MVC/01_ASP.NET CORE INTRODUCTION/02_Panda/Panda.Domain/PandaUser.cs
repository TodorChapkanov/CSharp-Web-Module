using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Panda.Domain
{
    public class PandaUser: IdentityUser
    {
        public PandaUser()
        {
           
            this.Receipts = new HashSet<Receipt>();
            this.Packages = new HashSet<Package>();
        }

        public PandaUserRole UserRole { get; set; }

        public ICollection<Package> Packages { get; set; }
        
        public ICollection<Receipt> Receipts { get; set; }
    }
}
