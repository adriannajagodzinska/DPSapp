namespace DPSapp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dodaniepacjentowdopokoju : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patient", "Room_RoomId", c => c.Int());
            CreateIndex("dbo.Patient", "Room_RoomId");
            AddForeignKey("dbo.Patient", "Room_RoomId", "dbo.Room", "RoomId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Patient", "Room_RoomId", "dbo.Room");
            DropIndex("dbo.Patient", new[] { "Room_RoomId" });
            DropColumn("dbo.Patient", "Room_RoomId");
        }
    }
}
