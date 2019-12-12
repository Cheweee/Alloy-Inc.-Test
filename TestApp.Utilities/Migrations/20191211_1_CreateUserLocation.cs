using FluentMigrator;

namespace TestApp.Utilities.Migrations
{
    [Migration(201912110001)]
    public class CreateUserLocation : Migration
    {
        public override void Down()
        {
            Delete.Table("UserLocation");
        }

        public override void Up()
        {
            Create.Table("UserLocation")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("IPAddress").AsString().NotNullable()
            .WithColumn("Location").AsString().NotNullable();
        }
    }
}