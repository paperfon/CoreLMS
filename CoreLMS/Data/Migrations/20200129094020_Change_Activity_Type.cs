using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreLMS.Data.Migrations
{
    public partial class Change_Activity_Type : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LMSUserCourse_Course_CourseId",
                table: "LMSUserCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_LMSUserCourse_AspNetUsers_LMSUserId",
                table: "LMSUserCourse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LMSUserCourse",
                table: "LMSUserCourse");

            migrationBuilder.RenameTable(
                name: "LMSUserCourse",
                newName: "LMSUserCourses");

            migrationBuilder.RenameIndex(
                name: "IX_LMSUserCourse_CourseId",
                table: "LMSUserCourses",
                newName: "IX_LMSUserCourses_CourseId");

            migrationBuilder.AlterColumn<int>(
                name: "ActivityType",
                table: "Activity",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LMSUserCourses",
                table: "LMSUserCourses",
                columns: new[] { "LMSUserId", "CourseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_LMSUserCourses_Course_CourseId",
                table: "LMSUserCourses",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LMSUserCourses_AspNetUsers_LMSUserId",
                table: "LMSUserCourses",
                column: "LMSUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LMSUserCourses_Course_CourseId",
                table: "LMSUserCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_LMSUserCourses_AspNetUsers_LMSUserId",
                table: "LMSUserCourses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LMSUserCourses",
                table: "LMSUserCourses");

            migrationBuilder.RenameTable(
                name: "LMSUserCourses",
                newName: "LMSUserCourse");

            migrationBuilder.RenameIndex(
                name: "IX_LMSUserCourses_CourseId",
                table: "LMSUserCourse",
                newName: "IX_LMSUserCourse_CourseId");

            migrationBuilder.AlterColumn<string>(
                name: "ActivityType",
                table: "Activity",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddPrimaryKey(
                name: "PK_LMSUserCourse",
                table: "LMSUserCourse",
                columns: new[] { "LMSUserId", "CourseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_LMSUserCourse_Course_CourseId",
                table: "LMSUserCourse",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LMSUserCourse_AspNetUsers_LMSUserId",
                table: "LMSUserCourse",
                column: "LMSUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
