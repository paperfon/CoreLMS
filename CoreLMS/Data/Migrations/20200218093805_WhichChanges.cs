using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreLMS.Data.Migrations
{
    public partial class WhichChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_AspNetUsers_LMSUserId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_LMSUserId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "LMSUserId",
                table: "Courses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LMSUserId",
                table: "Courses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_LMSUserId",
                table: "Courses",
                column: "LMSUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_AspNetUsers_LMSUserId",
                table: "Courses",
                column: "LMSUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
