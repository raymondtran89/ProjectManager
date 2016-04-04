using System.Data.Entity.Migrations;
using ProjectManager.DataAccessLayer.Migrations;

namespace ProjectManager.WebUI.Infrastructure
{
    public class EfConfig
    {
        public static void Initialize()
        {
            RunMigrations();
        }

        private static void RunMigrations()
        {
            var efMigrationSettings = new Configuration();
            var efMigrator = new DbMigrator(efMigrationSettings);
            efMigrator.Update();
        }
    }
}