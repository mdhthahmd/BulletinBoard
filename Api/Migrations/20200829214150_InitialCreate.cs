using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bulletins",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    HeadingText = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bulletins", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Bulletins",
                columns: new[] { "Id", "Content", "CreatedAt", "CreatedBy", "HeadingText", "Status" },
                values: new object[] { 1, "This is the Content of the Bulletin", new DateTime(2020, 8, 30, 2, 41, 50, 563, DateTimeKind.Local).AddTicks(1682), 1, "Bulletin Header", 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bulletins");
        }
    }
}
