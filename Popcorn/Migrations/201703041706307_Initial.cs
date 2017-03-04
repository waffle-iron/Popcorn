namespace Popcorn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MovieHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MovieEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MovieId = c.Int(nullable: false),
                        IsFavorite = c.Boolean(nullable: false),
                        HasBeenSeen = c.Boolean(nullable: false),
                        Url = c.String(maxLength: 4000),
                        ImdbCode = c.String(maxLength: 4000),
                        Title = c.String(maxLength: 4000),
                        TitleLong = c.String(maxLength: 4000),
                        Slug = c.String(maxLength: 4000),
                        Year = c.Int(nullable: false),
                        Rating = c.Double(nullable: false),
                        Runtime = c.Int(nullable: false),
                        Language = c.String(maxLength: 4000),
                        MpaRating = c.String(maxLength: 4000),
                        DownloadCount = c.Int(nullable: false),
                        LikeCount = c.Int(nullable: false),
                        DescriptionIntro = c.String(maxLength: 4000),
                        DescriptionFull = c.String(maxLength: 4000),
                        YtTrailerCode = c.String(maxLength: 4000),
                        BackdropImage = c.String(maxLength: 4000),
                        PosterImage = c.String(maxLength: 4000),
                        BackgroundImage = c.String(maxLength: 4000),
                        SmallCoverImage = c.String(maxLength: 4000),
                        MediumCoverImage = c.String(maxLength: 4000),
                        LargeCoverImage = c.String(maxLength: 4000),
                        MediumScreenshotImage1 = c.String(maxLength: 4000),
                        MediumScreenshotImage2 = c.String(maxLength: 4000),
                        MediumScreenshotImage3 = c.String(maxLength: 4000),
                        LargeScreenshotImage1 = c.String(maxLength: 4000),
                        LargeScreenshotImage2 = c.String(maxLength: 4000),
                        LargeScreenshotImage3 = c.String(maxLength: 4000),
                        DateUploaded = c.String(maxLength: 4000),
                        DateUploadedUnix = c.Int(nullable: false),
                        MovieHistory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MovieHistories", t => t.MovieHistory_Id)
                .Index(t => t.MovieHistory_Id);
            
            CreateTable(
                "dbo.Casts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                        CharacterName = c.String(maxLength: 4000),
                        SmallImage = c.String(maxLength: 4000),
                        ImdbCode = c.String(maxLength: 4000),
                        MovieEntity_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MovieEntities", t => t.MovieEntity_Id)
                .Index(t => t.MovieEntity_Id);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                        MovieEntity_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MovieEntities", t => t.MovieEntity_Id)
                .Index(t => t.MovieEntity_Id);
            
            CreateTable(
                "dbo.Torrents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(maxLength: 4000),
                        Hash = c.String(maxLength: 4000),
                        Quality = c.String(maxLength: 4000),
                        Seeds = c.Int(nullable: false),
                        Peers = c.Int(nullable: false),
                        Size = c.String(maxLength: 4000),
                        SizeBytes = c.Long(),
                        DateUploaded = c.String(maxLength: 4000),
                        DateUploadedUnix = c.Int(nullable: false),
                        MovieEntity_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MovieEntities", t => t.MovieEntity_Id)
                .Index(t => t.MovieEntity_Id);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LocalizedName = c.String(maxLength: 4000),
                        EnglishName = c.String(maxLength: 4000),
                        Culture = c.String(maxLength: 4000),
                        IsCurrentLanguage = c.Boolean(nullable: false),
                        Settings_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Settings", t => t.Settings_Id)
                .Index(t => t.Settings_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Languages", "Settings_Id", "dbo.Settings");
            DropForeignKey("dbo.MovieEntities", "MovieHistory_Id", "dbo.MovieHistories");
            DropForeignKey("dbo.Torrents", "MovieEntity_Id", "dbo.MovieEntities");
            DropForeignKey("dbo.Genres", "MovieEntity_Id", "dbo.MovieEntities");
            DropForeignKey("dbo.Casts", "MovieEntity_Id", "dbo.MovieEntities");
            DropIndex("dbo.Languages", new[] { "Settings_Id" });
            DropIndex("dbo.Torrents", new[] { "MovieEntity_Id" });
            DropIndex("dbo.Genres", new[] { "MovieEntity_Id" });
            DropIndex("dbo.Casts", new[] { "MovieEntity_Id" });
            DropIndex("dbo.MovieEntities", new[] { "MovieHistory_Id" });
            DropTable("dbo.Languages");
            DropTable("dbo.Settings");
            DropTable("dbo.Torrents");
            DropTable("dbo.Genres");
            DropTable("dbo.Casts");
            DropTable("dbo.MovieEntities");
            DropTable("dbo.MovieHistories");
        }
    }
}
