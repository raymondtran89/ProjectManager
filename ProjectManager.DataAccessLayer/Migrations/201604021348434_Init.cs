namespace ProjectManager.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.UserInRoles",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        UserKey = c.Guid(nullable: false),
                        RoleKey = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.Roles", t => t.RoleKey, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserKey, cascadeDelete: true)
                .Index(t => t.UserKey)
                .Index(t => t.RoleKey);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Key = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Salt = c.String(nullable: false),
                        HashedPassword = c.String(nullable: false),
                        IsLocked = c.Boolean(nullable: false),
                        CreateOn = c.DateTime(nullable: false),
                        LastUpdatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Key);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserInRoles", "UserKey", "dbo.Users");
            DropForeignKey("dbo.UserInRoles", "RoleKey", "dbo.Roles");
            DropIndex("dbo.UserInRoles", new[] { "RoleKey" });
            DropIndex("dbo.UserInRoles", new[] { "UserKey" });
            DropTable("dbo.Users");
            DropTable("dbo.UserInRoles");
            DropTable("dbo.Roles");
        }
    }
}
