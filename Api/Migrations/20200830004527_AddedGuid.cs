using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class AddedGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bulletins",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    HeadingText = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bulletins", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Bulletins",
                columns: new[] { "Id", "Content", "CreatedAt", "CreatedBy", "HeadingText", "Status" },
                values: new object[] { new Guid("24f495ca-ea5f-4a96-b7ce-c46c031a0682"), "This is the Content of the Bulletin", new DateTime(2020, 8, 30, 5, 45, 27, 22, DateTimeKind.Local).AddTicks(9990), 1, "Bulletin Header", 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bulletins");
        }
    }
}
