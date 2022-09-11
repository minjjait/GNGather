using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class RegionDbFestivalDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    RegionDbId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegionName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.RegionDbId);
                });

            migrationBuilder.CreateTable(
                name: "Festival",
                columns: table => new
                {
                    FestivalDbId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FestivalName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RegionDbId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Festival", x => x.FestivalDbId);
                    table.ForeignKey(
                        name: "FK_Festival_Region_RegionDbId",
                        column: x => x.RegionDbId,
                        principalTable: "Region",
                        principalColumn: "RegionDbId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Festival_FestivalName",
                table: "Festival",
                column: "FestivalName",
                unique: true,
                filter: "[FestivalName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Festival_RegionDbId",
                table: "Festival",
                column: "RegionDbId");

            migrationBuilder.CreateIndex(
                name: "IX_Region_RegionName",
                table: "Region",
                column: "RegionName",
                unique: true,
                filter: "[RegionName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Festival");

            migrationBuilder.DropTable(
                name: "Region");
        }
    }
}
