namespace ClothBazar.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedCategoryIDinProduct : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "category_ID", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "category_ID" });
            RenameColumn(table: "dbo.Products", name: "category_ID", newName: "CategoryID");
            AlterColumn("dbo.Products", "CategoryID", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "CategoryID");
            AddForeignKey("dbo.Products", "CategoryID", "dbo.Categories", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "CategoryID" });
            AlterColumn("dbo.Products", "CategoryID", c => c.Int());
            RenameColumn(table: "dbo.Products", name: "CategoryID", newName: "category_ID");
            CreateIndex("dbo.Products", "category_ID");
            AddForeignKey("dbo.Products", "category_ID", "dbo.Categories", "ID");
        }
    }
}
