using System.Data.Entity;
using ProjectManager.DataAccessLayer.Entity;

namespace ProjectManager.DataAccessLayer.Infractructure
{
    public class EntitiesContext : DbContext
    {
        public EntitiesContext()
            : base("ProjectManager")
        {
        }

        public IDbSet<User> Users { get; set; }
        public IDbSet<Role> Roles { get; set; }
        public IDbSet<UserInRole> UserInRoles { get; set; }

        public IDbSet<Test> Tests { get; set; }
    }
}