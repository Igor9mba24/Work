using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Work.Migrations
{
    /// <inheritdoc />
    public partial class god2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Persons_PersonId",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_PersonId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Skills");

            migrationBuilder.CreateTable(
                name: "Skill_People",
                columns: table => new
                {
                    PersonId = table.Column<long>(type: "bigint", nullable: false),
                    SkillId = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill_People", x => new { x.PersonId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_Skill_People_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Skill_People_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Level",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Skill_People_SkillId",
                table: "Skill_People",
                column: "SkillId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Skill_People");

            migrationBuilder.AddColumn<long>(
                name: "PersonId",
                table: "Skills",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_PersonId",
                table: "Skills",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Persons_PersonId",
                table: "Skills",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id");
        }
    }
}
