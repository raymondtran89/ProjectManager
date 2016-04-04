using System.Collections.Generic;
using ProjectManager.DataAccessLayer.Entity;

namespace ProjectManager.ServiceLayer.Infractructure
{
    public class UserWithRoles
    {
        public User User { get; set; }
        public IEnumerable<Role> Roles { get; set; }
    }
}