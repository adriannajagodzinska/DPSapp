namespace DPSapp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodaniepomocnejzmiennejdoUsera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "PatientName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "PatientName");
        }
    }
}
