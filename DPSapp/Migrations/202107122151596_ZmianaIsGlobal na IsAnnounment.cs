namespace DPSapp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaIsGlobalnaIsAnnounment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tag", "IsAnnouncement", c => c.Boolean(nullable: false));
            DropColumn("dbo.Tag", "IsGlobal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tag", "IsGlobal", c => c.Boolean(nullable: false));
            DropColumn("dbo.Tag", "IsAnnouncement");
        }
    }
}
