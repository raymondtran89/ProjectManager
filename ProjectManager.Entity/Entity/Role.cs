using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProjectManager.Entity.Entity.Abtract;

namespace ProjectManager.Entity.Entity
{
    public sealed class Role : IEntity
    {
        public Role()
        {
            UserInRoles = new HashSet<UserInRole>();
        }

        [Required]
        public string Name { get; set; }

        public ICollection<UserInRole> UserInRoles { get; set; }

        [Key]
        public Guid Key { get; set; }
    }
}