namespace E_Recarga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        StationId = c.Int(),
                        PodId = c.Int(),
                        UserId = c.String(maxLength: 128),
                        Cost = c.Double(nullable: false),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        AppointmentStatusId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Pods", t => t.PodId)
                .ForeignKey("dbo.Stations", t => t.StationId)
                .ForeignKey("dbo.AppointmentStatus", t => t.AppointmentStatusId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.CompanyId)
                .Index(t => t.StationId)
                .Index(t => t.PodId)
                .Index(t => t.UserId)
                .Index(t => t.AppointmentStatusId);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        PhoneNumber = c.String(nullable: false),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 100),
                        Wallet = c.Double(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        StationId = c.Int(),
                        CompanyId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Stations", t => t.StationId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.StationId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Stations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        ComercialName = c.String(nullable: false, maxLength: 100),
                        StreetName = c.String(nullable: false, maxLength: 100),
                        BuildingNumber = c.Int(nullable: false),
                        PostalCode = c.String(nullable: false),
                        Parish = c.String(nullable: false, maxLength: 100),
                        Region = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Pods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StationId = c.Int(nullable: false),
                        PodId = c.Int(nullable: false),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PodTypes", t => t.PodId)
                .ForeignKey("dbo.Stations", t => t.StationId, cascadeDelete: true)
                .Index(t => t.StationId)
                .Index(t => t.PodId);
            
            CreateTable(
                "dbo.PodTypes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Prices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StationId = c.Int(nullable: false),
                        Time = c.Time(nullable: false, precision: 7),
                        CostNormal = c.Double(nullable: false),
                        CostUltra = c.Double(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stations", t => t.StationId, cascadeDelete: true)
                .Index(t => t.StationId);
            
            CreateTable(
                "dbo.AppointmentStatus",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Appointments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Appointments", "AppointmentStatusId", "dbo.AppointmentStatus");
            DropForeignKey("dbo.Appointments", "StationId", "dbo.Stations");
            DropForeignKey("dbo.Appointments", "PodId", "dbo.Pods");
            DropForeignKey("dbo.Appointments", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.AspNetUsers", "StationId", "dbo.Stations");
            DropForeignKey("dbo.Prices", "StationId", "dbo.Stations");
            DropForeignKey("dbo.Pods", "StationId", "dbo.Stations");
            DropForeignKey("dbo.Pods", "PodId", "dbo.PodTypes");
            DropForeignKey("dbo.Stations", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.AspNetUsers", "CompanyId", "dbo.Companies");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Prices", new[] { "StationId" });
            DropIndex("dbo.Pods", new[] { "PodId" });
            DropIndex("dbo.Pods", new[] { "StationId" });
            DropIndex("dbo.Stations", new[] { "CompanyId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "CompanyId" });
            DropIndex("dbo.AspNetUsers", new[] { "StationId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Appointments", new[] { "AppointmentStatusId" });
            DropIndex("dbo.Appointments", new[] { "UserId" });
            DropIndex("dbo.Appointments", new[] { "PodId" });
            DropIndex("dbo.Appointments", new[] { "StationId" });
            DropIndex("dbo.Appointments", new[] { "CompanyId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AppointmentStatus");
            DropTable("dbo.Prices");
            DropTable("dbo.PodTypes");
            DropTable("dbo.Pods");
            DropTable("dbo.Stations");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Companies");
            DropTable("dbo.Appointments");
        }
    }
}
