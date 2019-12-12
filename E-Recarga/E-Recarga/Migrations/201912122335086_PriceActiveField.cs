namespace E_Recarga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PriceActiveField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prices", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prices", "Active");
        }
    }
}
