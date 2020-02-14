using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreLMS.Data.Migrations
{
    public partial class AddingCoursesToUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_AspNetUsers_LMSUserId",
                table: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_LMSUserId",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "LMSUserId",
                table: "AspNetRoles");

            migrationBuilder.AddColumn<string>(
                name: "LMSUserId",
                table: "Courses",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "LMSUserId",
                table: "AspNetRoles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_LMSUserId",
                table: "AspNetRoles",
                column: "LMSUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_AspNetUsers_LMSUserId",
                table: "AspNetRoles",
                column: "LMSUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
