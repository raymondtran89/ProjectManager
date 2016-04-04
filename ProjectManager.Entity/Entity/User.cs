using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProjectManager.Entity.Entity.Abtract;

namespace ProjectManager.Entity.Entity
{
    public sealed class User : IEntity
    {
        public User()
        {
            UserInRoles = new HashSet<UserInRole>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Salt { get; set; }

        [Required]
        public string HashedPassword { get; set; }

        public bool IsLocked { get; set; }

        public DateTime CreateOn { get; set; }
        public DateTime? LastUpdatedOn { get; set; }

        public ICollection<UserInRole> UserInRoles { get; set; }

        [Key]
        public Guid Key { get; set; }
    }
}