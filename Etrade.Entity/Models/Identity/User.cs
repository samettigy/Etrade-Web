using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etrade.Entity.Models.Identity
{
    public class User : IdentityUser<int>
    {
        public User()
        {

        }

        public User(string userName) : base(userName)
        {
        }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }=DateTime.Now;
    }
}
