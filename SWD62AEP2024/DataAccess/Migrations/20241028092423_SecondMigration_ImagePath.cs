using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class SecondMigration_ImagePath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SubjectFK",
                table: "Attendances",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "StudentFK",
                table: "Attendances",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentFK",
                table: "Attendances",
                column: "StudentFK");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_SubjectFK",
                table: "Attendances",
                column: "SubjectFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Students_StudentFK",
                table: "Attendances",
                column: "StudentFK",
                principalTable: "Students",
                principalColumn: "IdCard",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Subjects_SubjectFK",
                table: "Attendances",
                column: "SubjectFK",
                principalTable: "Subjects",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Students_StudentFK",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Subjects_SubjectFK",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_StudentFK",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_SubjectFK",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Students");

            migrationBuilder.AlterColumn<string>(
                name: "SubjectFK",
                table: "Attendances",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "StudentFK",
                table: "Attendances",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
