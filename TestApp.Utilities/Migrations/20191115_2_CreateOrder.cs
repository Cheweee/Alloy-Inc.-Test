using FluentMigrator;

namespace TestApp.Utilities.Migrations
{
    [Migration(201911150002)]
    public class CreateOrder : Migration
    {
        public override void Down()
        {
            Delete.Table("Order");
        }

        public override void Up()
        {
            Create.Table("Order")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("DeliveryId").AsInt32().ForeignKey("Delivery", "Id")
            .WithColumn("PaymentMethod").AsInt32().NotNullable()
            .WithColumn("FullName").AsString().NotNullable()
            .WithColumn("Addres").AsString().NotNullable()
            .WithColumn("SpecialDate").AsDateTime2().Nullable()
            .WithColumn("DateCreated").AsDateTime2().WithDefault(SystemMethods.CurrentUTCDateTime);
        }
    }
}