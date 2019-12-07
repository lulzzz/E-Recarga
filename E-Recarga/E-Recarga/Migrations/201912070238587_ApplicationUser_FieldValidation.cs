namespace E_Recarga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUser_FieldValidation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Name", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Name", c => c.String());
        }
    }
}
