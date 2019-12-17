using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Events.Data.Migrations
{
    public partial class updatedatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Staffs",
                schema: "thamco.events",
                columns: table => new
                {
                    StaffId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: false),
                    Surname = table.Column<string>(nullable: false),
                    FirstAider = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.StaffId);
                });

            migrationBuilder.InsertData(
                schema: "thamco.events",
                table: "Staffs",
                columns: new[] { "StaffId", "FirstAider", "FirstName", "Surname" },
                values: new object[] { 1, true, "Jack", "Ferguson" });

            migrationBuilder.InsertData(
                schema: "thamco.events",
                table: "Staffs",
                columns: new[] { "StaffId", "FirstAider", "FirstName", "Surname" },
                values: new object[] { 2, true, "John", "Boyd" });

            migrationBuilder.InsertData(
                schema: "thamco.events",
                table: "Staffs",
                columns: new[] { "StaffId", "FirstAider", "FirstName", "Surname" },
                values: new object[] { 3, false, "James", "Francis" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Staffs",
                schema: "thamco.events");
        }
    }
}
