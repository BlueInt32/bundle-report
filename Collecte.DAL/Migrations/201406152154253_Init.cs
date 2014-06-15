namespace Collecte.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnswerChoices",
                c => new
                    {
                        AnswerChoiceId = c.Int(nullable: false, identity: true),
                        UserId = c.Guid(),
                        QuestionNumber = c.Int(nullable: false),
                        AnswerChosen = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AnswerChoiceId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LastName = c.String(nullable: false, maxLength: 100),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 200),
                        Civilite = c.String(maxLength: 3),
                        Address = c.String(maxLength: 300),
                        NumeroVoie = c.String(maxLength: 10),
                        TypeVoie = c.String(maxLength: 20),
                        NomVoie = c.String(maxLength: 200),
                        Zipcode = c.Int(nullable: false),
                        City = c.String(maxLength: 200),
                        Phone = c.String(maxLength: 10),
                        IsCanal = c.Boolean(nullable: false),
                        IsOffreGroupCanal = c.Boolean(nullable: false),
                        FriendEmail1 = c.String(maxLength: 200),
                        FriendEmail2 = c.String(maxLength: 200),
                        FriendEmail3 = c.String(maxLength: 200),
                        HasSentEmailsToFriends = c.Boolean(nullable: false),
                        HeroicScore = c.Int(nullable: false),
                        HeroicStatus = c.Int(nullable: false),
                        ChancesByStep = c.String(maxLength: 8),
                        ChancesAmount = c.Short(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ParticipationDate = c.DateTime(),
                        Provenance = c.String(maxLength: 25),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InstantGagnants",
                c => new
                    {
                        InstantGagnantId = c.Int(nullable: false, identity: true),
                        LotId = c.Int(nullable: false),
                        StartDateTime = c.DateTime(nullable: false),
                        Won = c.Boolean(nullable: false),
                        WonDate = c.DateTime(),
                        Label = c.String(),
                        FrontHtmlId = c.String(),
                        UserId = c.Guid(),
                    })
                .PrimaryKey(t => t.InstantGagnantId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.BundleFiles",
                c => new
                    {
                        BundleFileId = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        FileName = c.String(maxLength: 255),
                        CreationDate = c.DateTime(nullable: false),
                        BundleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BundleFileId)
                .ForeignKey("dbo.Bundles", t => t.BundleId, cascadeDelete: true)
                .Index(t => t.BundleId);
            
            CreateTable(
                "dbo.Bundles",
                c => new
                    {
                        BundleId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        NbInscriptions = c.Int(),
                        NbRetoursCanal = c.Int(),
                        NbOk = c.Int(),
                        NbKo = c.Int(),
                    })
                .PrimaryKey(t => t.BundleId);
            
            CreateTable(
                "dbo.TradeOccurences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BundleFiles", "BundleId", "dbo.Bundles");
            DropForeignKey("dbo.InstantGagnants", "UserId", "dbo.Users");
            DropForeignKey("dbo.AnswerChoices", "UserId", "dbo.Users");
            DropIndex("dbo.BundleFiles", new[] { "BundleId" });
            DropIndex("dbo.InstantGagnants", new[] { "UserId" });
            DropIndex("dbo.AnswerChoices", new[] { "UserId" });
            DropTable("dbo.TradeOccurences");
            DropTable("dbo.Bundles");
            DropTable("dbo.BundleFiles");
            DropTable("dbo.InstantGagnants");
            DropTable("dbo.Users");
            DropTable("dbo.AnswerChoices");
        }
    }
}
