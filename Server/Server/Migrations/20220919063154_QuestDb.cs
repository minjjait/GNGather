using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class QuestDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quest",
                columns: table => new
                {
                    QuestDbId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateId = table.Column<int>(type: "int", nullable: false),
                    IsCleared = table.Column<bool>(type: "bit", nullable: false),
                    OwnerDbId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quest", x => x.QuestDbId);
                    table.ForeignKey(
                        name: "FK_Quest_Player_OwnerDbId",
                        column: x => x.OwnerDbId,
                        principalTable: "Player",
                        principalColumn: "PlayerDbId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quest_OwnerDbId",
                table: "Quest",
                column: "OwnerDbId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Quest");
        }
    }
}
