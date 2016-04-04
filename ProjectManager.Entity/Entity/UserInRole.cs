using System;
using System.ComponentModel.DataAnnotations;
using ProjectManager.Entity.Entity.Abtract;

namespace ProjectManager.Entity.Entity
{
    public class UserInRole : IEntity
    {
        public Guid UserKey { get; set; }
        public Guid RoleKey { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }

        [Key]
        public Guid Key { get; set; }
    }
}