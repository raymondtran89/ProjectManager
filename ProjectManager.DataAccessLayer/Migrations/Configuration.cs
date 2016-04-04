using System;
using System.Data.Entity.Migrations;
using ProjectManager.DataAccessLayer.Entity;
using ProjectManager.DataAccessLayer.Infractructure;

namespace ProjectManager.DataAccessLayer.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<EntitiesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(EntitiesContext context)
        {
            context.Roles.AddOrUpdate(
                new Role { Key = Guid.NewGuid(), Name = "Admin" },
                new Role { Key = Guid.NewGuid(), Name = "TeamLeader" },
                new Role { Key = Guid.NewGuid(), Name = "Developer" },
                new Role { Key = Guid.NewGuid(), Name = "Tester" },
                new Role { Key = Guid.NewGuid(), Name = "Secretary" }
                );

            context.Tests.AddOrUpdate(
                new Test { Id = 1, Name = "Admin" },
                new Test { Id = 2, Name = "TeamLeader" },
                new Test { Id = 3, Name = "Developer" },
                new Test { Id = 4, Name = "Tester" },
                new Test { Id = 5, Name = "Secretary" }
                );
        }
    }
}