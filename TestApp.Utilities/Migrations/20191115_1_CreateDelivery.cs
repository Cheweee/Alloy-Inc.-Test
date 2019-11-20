using FluentMigrator;

namespace TestApp.Utilities.Migrations
{
    [Migration(201911150001)]
    public class CreateDeliveryService : Migration
    {
        public override void Down()
        {
            Delete.Table("Delivery");
        }

        public override void Up()
        {
            Create.Table("Delivery")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("DeliveryPrice").AsDouble().NotNullable()
            .WithColumn("Name").AsString().NotNullable();
        }
    }
}