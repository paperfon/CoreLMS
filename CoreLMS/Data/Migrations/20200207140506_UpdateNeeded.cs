using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreLMS.Data.Migrations
{
    public partial class UpdateNeeded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_Module_ModuleId",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_Document_Activity_ActivityId",
                table: "Document");

            migrationBuilder.DropForeignKey(
                name: "FK_Document_Course_CourseId",
                table: "Document");

            migrationBuilder.DropForeignKey(
                name: "FK_Document_AspNetUsers_LMSUserId",
                table: "Document");

            migrationBuilder.DropForeignKey(
                name: "FK_Document_Module_ModuleId",
                table: "Document");

            migrationBuilder.DropForeignKey(
                name: "FK_LMSUserCourses_Course_CourseId",
                table: "LMSUserCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_Module_Course_CourseId",
                table: "Module");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Module",
                table: "Module");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Document",
                table: "Document");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Course",
                table: "Course");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activity",
                table: "Activity");

            migrationBuilder.RenameTable(
                name: "Module",
                newName: "Modules");

            migrationBuilder.RenameTable(
                name: "Document",
                newName: "Documents");

            migrationBuilder.RenameTable(
                name: "Course",
                newName: "Courses");

            migrationBuilder.RenameTable(
                name: "Activity",
                newName: "Activities");

            migrationBuilder.RenameIndex(
                name: "IX_Module_CourseId",
                table: "Modules",
                newName: "IX_Modules_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Document_ModuleId",
                table: "Documents",
                newName: "IX_Documents_ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_Document_LMSUserId",
                table: "Documents",
                newName: "IX_Documents_LMSUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Document_CourseId",
                table: "Documents",
                newName: "IX_Documents_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Document_ActivityId",
                table: "Documents",
                newName: "IX_Documents_ActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_Activity_ModuleId",
                table: "Activities",
                newName: "IX_Activities_ModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Modules",
                table: "Modules",
                column: "ModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Documents",
                table: "Documents",
                column: "DocumentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                column: "CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activities",
                table: "Activities",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Modules_ModuleId",
                table: "Activities",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "ModuleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Activities_ActivityId",
                table: "Documents",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "ActivityId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Courses_CourseId",
                table: "Documents",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_AspNetUsers_LMSUserId",
                table: "Documents",
                column: "LMSUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Modules_ModuleId",
                table: "Documents",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "ModuleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LMSUserCourses_Courses_CourseId",
                table: "LMSUserCourses",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Courses_CourseId",
                table: "Modules",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Modules_ModuleId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Activities_ActivityId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Courses_CourseId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_AspNetUsers_LMSUserId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Modules_ModuleId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_LMSUserCourses_Courses_CourseId",
                table: "LMSUserCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Courses_CourseId",
                table: "Modules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Modules",
                table: "Modules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Documents",
                table: "Documents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activities",
                table: "Activities");

            migrationBuilder.RenameTable(
                name: "Modules",
                newName: "Module");

            migrationBuilder.RenameTable(
                name: "Documents",
                newName: "Document");

            migrationBuilder.RenameTable(
                name: "Courses",
                newName: "Course");

            migrationBuilder.RenameTable(
                name: "Activities",
                newName: "Activity");

            migrationBuilder.RenameIndex(
                name: "IX_Modules_CourseId",
                table: "Module",
                newName: "IX_Module_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_ModuleId",
                table: "Document",
                newName: "IX_Document_ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_LMSUserId",
                table: "Document",
                newName: "IX_Document_LMSUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_CourseId",
                table: "Document",
                newName: "IX_Document_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_ActivityId",
                table: "Document",
                newName: "IX_Document_ActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_ModuleId",
                table: "Activity",
                newName: "IX_Activity_ModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Module",
                table: "Module",
                column: "ModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Document",
                table: "Document",
                column: "DocumentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Course",
                table: "Course",
                column: "CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activity",
                table: "Activity",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_Module_ModuleId",
                table: "Activity",
                column: "ModuleId",
                principalTable: "Module",
                principalColumn: "ModuleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Activity_ActivityId",
                table: "Document",
                column: "ActivityId",
                principalTable: "Activity",
                principalColumn: "ActivityId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Course_CourseId",
                table: "Document",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Document_AspNetUsers_LMSUserId",
                table: "Document",
                column: "LMSUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Module_ModuleId",
                table: "Document",
                column: "ModuleId",
                principalTable: "Module",
                principalColumn: "ModuleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LMSUserCourses_Course_CourseId",
                table: "LMSUserCourses",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Module_Course_CourseId",
                table: "Module",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
