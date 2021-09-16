namespace DPSapp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Zmianausuwaniatagu : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TagMessage", "Tag_TagId", "dbo.Tag");
            DropForeignKey("dbo.TagMessage", "Message_MessageId", "dbo.Message");
            DropIndex("dbo.TagMessage", new[] { "Tag_TagId" });
            DropIndex("dbo.TagMessage", new[] { "Message_MessageId" });
            AddColumn("dbo.Tag", "Messages_MessageId", c => c.Int());
            AddColumn("dbo.Tag", "Message_MessageId", c => c.Int());
            CreateIndex("dbo.Tag", "Messages_MessageId");
            CreateIndex("dbo.Tag", "Message_MessageId");
            AddForeignKey("dbo.Tag", "Messages_MessageId", "dbo.Message", "MessageId", cascadeDelete: true);
            AddForeignKey("dbo.Tag", "Message_MessageId", "dbo.Message", "MessageId");
            DropTable("dbo.TagMessage");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TagMessage",
                c => new
                    {
                        Tag_TagId = c.Int(nullable: false),
                        Message_MessageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_TagId, t.Message_MessageId });
            
            DropForeignKey("dbo.Tag", "Message_MessageId", "dbo.Message");
            DropForeignKey("dbo.Tag", "Messages_MessageId", "dbo.Message");
            DropIndex("dbo.Tag", new[] { "Message_MessageId" });
            DropIndex("dbo.Tag", new[] { "Messages_MessageId" });
            DropColumn("dbo.Tag", "Message_MessageId");
            DropColumn("dbo.Tag", "Messages_MessageId");
            CreateIndex("dbo.TagMessage", "Message_MessageId");
            CreateIndex("dbo.TagMessage", "Tag_TagId");
            AddForeignKey("dbo.TagMessage", "Message_MessageId", "dbo.Message", "MessageId", cascadeDelete: true);
            AddForeignKey("dbo.TagMessage", "Tag_TagId", "dbo.Tag", "TagId", cascadeDelete: true);
        }
    }
}
