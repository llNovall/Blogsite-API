using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogsiteDomain.Entities.Account
{
    public class AppUser : IdentityUser
    {
        public AppUser(string userName) : base(userName)
        {
        }
    }
}