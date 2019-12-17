using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Events.Data.Migrations
{
    public partial class VenueCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VenueCode",
                schema: "thamco.events",
                table: "Events",
                maxLength: 5,
                nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "AvailableVenueModels",
            //    schema: "thamco.events",
            //    columns: table => new
            //    {
            //        Code = table.Column<string>(maxLength: 5, nullable: false),
            //        Name = table.Column<string>(nullable: false),
            //        Description = table.Column<string>(nullable: false),
            //        Capacity = table.Column<int>(nullable: false),
            //        date = table.Column<DateTime>(nullable: false),
            //        CostPerHour = table.Column<double>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AvailableVenueModels", x => x.Code);
            //    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "AvailableVenueModels",
            //    schema: "thamco.events");

            migrationBuilder.DropColumn(
                name: "VenueCode",
                schema: "thamco.events",
                table: "Events");
        }
    }
}
