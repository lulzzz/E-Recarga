namespace E_Recarga.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PodIdentifierField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pods", "Identifier", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pods", "Identifier");
        }
    }
}
