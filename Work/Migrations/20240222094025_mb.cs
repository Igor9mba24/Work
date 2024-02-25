using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Work.Migrations
{
    /// <inheritdoc />
    public partial class mb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skill_People_Persons_PersonId",
                table: "Skill_People");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_People_Skills_SkillId",
                table: "Skill_People");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skill_People",
                table: "Skill_People");

            migrationBuilder.RenameTable(
                name: "Skill_People",
                newName: "Skill_Person");

            migrationBuilder.RenameIndex(
                name: "IX_Skill_People_SkillId",
                table: "Skill_Person",
                newName: "IX_Skill_Person_SkillId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skill_Person",
                table: "Skill_Person",
                columns: new[] { "PersonId", "SkillId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_Person_Persons_PersonId",
                table: "Skill_Person",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_Person_Skills_SkillId",
                table: "Skill_Person",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Level",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skill_Person_Persons_PersonId",
                table: "Skill_Person");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_Person_Skills_SkillId",
                table: "Skill_Person");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skill_Person",
                table: "Skill_Person");

            migrationBuilder.RenameTable(
                name: "Skill_Person",
                newName: "Skill_People");

            migrationBuilder.RenameIndex(
                name: "IX_Skill_Person_SkillId",
                table: "Skill_People",
                newName: "IX_Skill_People_SkillId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skill_People",
                table: "Skill_People",
                columns: new[] { "PersonId", "SkillId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_People_Persons_PersonId",
                table: "Skill_People",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_People_Skills_SkillId",
                table: "Skill_People",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Level",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
