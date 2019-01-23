using Microsoft.EntityFrameworkCore.Migrations;

namespace ASR.Migrations
{
    public partial class SchoolIDkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SchoolID",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_SchoolID",
                table: "AspNetUsers",
                column: "SchoolID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_SchoolID",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "SchoolID",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
