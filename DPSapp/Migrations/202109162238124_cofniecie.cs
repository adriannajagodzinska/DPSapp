namespace DPSapp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cofniecie : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tag", "Messages_MessageId", "dbo.Message");
            DropForeignKey("dbo.Tag", "Message_MessageId", "dbo.Message");
            DropIndex("dbo.Tag", new[] { "Messages_MessageId" });
            DropIndex("dbo.Tag", new[] { "Message_MessageId" });
            CreateTable(
                "dbo.TagMessage",
                c => new
                    {
                        Tag_TagId = c.Int(nullable: false),
                        Message_MessageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_TagId, t.Message_MessageId })
                .ForeignKey("dbo.Tag", t => t.Tag_TagId, cascadeDelete: true)
                .ForeignKey("dbo.Message", t => t.Message_MessageId, cascadeDelete: true)
                .Index(t => t.Tag_TagId)
                .Index(t => t.Message_MessageId);
            
            DropColumn("dbo.Tag", "Messages_MessageId");
            DropColumn("dbo.Tag", "Message_MessageId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tag", "Message_MessageId", c => c.Int());
            AddColumn("dbo.Tag", "Messages_MessageId", c => c.Int());
            DropForeignKey("dbo.TagMessage", "Message_MessageId", "dbo.Message");
            DropForeignKey("dbo.TagMessage", "Tag_TagId", "dbo.Tag");
            DropIndex("dbo.TagMessage", new[] { "Message_MessageId" });
            DropIndex("dbo.TagMessage", new[] { "Tag_TagId" });
            DropTable("dbo.TagMessage");
            CreateIndex("dbo.Tag", "Message_MessageId");
            CreateIndex("dbo.Tag", "Messages_MessageId");
            AddForeignKey("dbo.Tag", "Message_MessageId", "dbo.Message", "MessageId");
            AddForeignKey("dbo.Tag", "Messages_MessageId", "dbo.Message", "MessageId", cascadeDelete: true);
        }
    }
}
