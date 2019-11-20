using System.Data;
using FluentMigrator;

namespace TestApp.Utilities.Migrations
{
    [Migration(201911150003)]
    public class CreateProduct : Migration
    {
        public override void Down()
        {
            Delete.Table("Product");
            Delete.Table("Cart");
        }

        public override void Up()
        {
            Create.Table("Product")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Count").AsInt32().Nullable()
            .WithColumn("Price").AsDouble().NotNullable()
            .WithColumn("Name").AsString().NotNullable()
            .WithColumn("DateCreated").AsDateTime2().WithDefault(SystemMethods.CurrentUTCDateTime);
            
            Create.Table("Cart")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("ProductId").AsInt32().Nullable().ForeignKey("Product", "Id").OnDeleteOrUpdate(Rule.None)
            .WithColumn("OrderId").AsInt32().Nullable().ForeignKey("Order", "Id")
            .WithColumn("Count").AsInt32().Nullable()
            .WithColumn("Price").AsDouble().NotNullable()
            .WithColumn("Name").AsString().NotNullable()
            .WithColumn("DateCreated").AsDateTime2().WithDefault(SystemMethods.CurrentUTCDateTime);
        }
    }
}