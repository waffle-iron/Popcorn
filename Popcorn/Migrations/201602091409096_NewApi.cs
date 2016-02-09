namespace Popcorn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewApi : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Actors", newName: "Casts");
            DropForeignKey("dbo.Directors", "MovieFull_Id", "dbo.MovieFulls");
            DropForeignKey("dbo.MovieFulls", "Images_Id", "dbo.Images");
            DropIndex("dbo.MovieFulls", new[] { "Images_Id" });
            DropIndex("dbo.Directors", new[] { "MovieFull_Id" });
            AddColumn("dbo.MovieFulls", "BackgroundImage", c => c.String(maxLength: 4000));
            AddColumn("dbo.MovieFulls", "SmallCoverImage", c => c.String(maxLength: 4000));
            AddColumn("dbo.MovieFulls", "MediumCoverImage", c => c.String(maxLength: 4000));
            AddColumn("dbo.MovieFulls", "LargeCoverImage", c => c.String(maxLength: 4000));
            AddColumn("dbo.MovieFulls", "MediumScreenshotImage1", c => c.String(maxLength: 4000));
            AddColumn("dbo.MovieFulls", "MediumScreenshotImage2", c => c.String(maxLength: 4000));
            AddColumn("dbo.MovieFulls", "MediumScreenshotImage3", c => c.String(maxLength: 4000));
            AddColumn("dbo.MovieFulls", "LargeScreenshotImage1", c => c.String(maxLength: 4000));
            AddColumn("dbo.MovieFulls", "LargeScreenshotImage2", c => c.String(maxLength: 4000));
            AddColumn("dbo.MovieFulls", "LargeScreenshotImage3", c => c.String(maxLength: 4000));
            DropColumn("dbo.MovieFulls", "Images_Id");
            DropColumn("dbo.Casts", "MediumImage");
            DropTable("dbo.Directors");
            DropTable("dbo.Images");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Directors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                        SmallImage = c.String(maxLength: 4000),
                        MediumImage = c.String(maxLength: 4000),
                        SmallImagePath = c.String(maxLength: 4000),
                        MovieFull_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Casts", "MediumImage", c => c.String(maxLength: 4000));
            AddColumn("dbo.MovieFulls", "Images_Id", c => c.Int());
            DropColumn("dbo.MovieFulls", "LargeScreenshotImage3");
            DropColumn("dbo.MovieFulls", "LargeScreenshotImage2");
            DropColumn("dbo.MovieFulls", "LargeScreenshotImage1");
            DropColumn("dbo.MovieFulls", "MediumScreenshotImage3");
            DropColumn("dbo.MovieFulls", "MediumScreenshotImage2");
            DropColumn("dbo.MovieFulls", "MediumScreenshotImage1");
            DropColumn("dbo.MovieFulls", "LargeCoverImage");
            DropColumn("dbo.MovieFulls", "MediumCoverImage");
            DropColumn("dbo.MovieFulls", "SmallCoverImage");
            DropColumn("dbo.MovieFulls", "BackgroundImage");
            CreateIndex("dbo.Directors", "MovieFull_Id");
            CreateIndex("dbo.MovieFulls", "Images_Id");
            AddForeignKey("dbo.MovieFulls", "Images_Id", "dbo.Images", "Id");
            AddForeignKey("dbo.Directors", "MovieFull_Id", "dbo.MovieFulls", "Id");
            RenameTable(name: "dbo.Casts", newName: "Actors");
        }
    }
}
