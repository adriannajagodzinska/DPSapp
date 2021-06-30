namespace DPSapp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodanieisGlobaldotgutagdlawszystkich : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tag", "IsGlobal", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tag", "IsGlobal");
        }
    }
}
