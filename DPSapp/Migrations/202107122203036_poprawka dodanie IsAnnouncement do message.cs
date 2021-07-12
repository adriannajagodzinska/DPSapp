namespace DPSapp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class poprawkadodanieIsAnnouncementdomessage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Message", "IsAnnouncement", c => c.Boolean(nullable: false));
            DropColumn("dbo.Tag", "IsAnnouncement");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tag", "IsAnnouncement", c => c.Boolean(nullable: false));
            DropColumn("dbo.Message", "IsAnnouncement");
        }
    }
}
