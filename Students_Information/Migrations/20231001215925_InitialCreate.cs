using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Students_Information.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsRegular = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CourseFee = table.Column<decimal>(type: "money", nullable: false),
                    AdmissionDate = table.Column<DateTime>(type: "date", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseId);
                    table.ForeignKey(
                        name: "FK_Courses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentId", "Age", "Gender", "IsRegular", "Picture", "StudentName" },
                values: new object[,]
                {
                    { 1, 25, 4, null, "1jpg", "Student 1" },
                    { 2, 11, 4, null, "2jpg", "Student 2" },
                    { 3, 27, 2, null, "3jpg", "Student 3" },
                    { 4, 11, 4, null, "4jpg", "Student 4" },
                    { 5, 27, 3, null, "5jpg", "Student 5" }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "CourseId", "AdmissionDate", "CourseFee", "CourseName", "StudentId" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 5, 29, 3, 59, 25, 201, DateTimeKind.Local).AddTicks(6229), 460m, "Course1", 1 },
                    { 2, new DateTime(2022, 8, 23, 3, 59, 25, 201, DateTimeKind.Local).AddTicks(6288), 894m, "Course2", 2 },
                    { 3, new DateTime(2022, 6, 10, 3, 59, 25, 201, DateTimeKind.Local).AddTicks(6310), 1293m, "Course3", 3 },
                    { 4, new DateTime(2022, 8, 23, 3, 59, 25, 201, DateTimeKind.Local).AddTicks(6328), 1900m, "Course4", 4 },
                    { 5, new DateTime(2022, 8, 8, 3, 59, 25, 201, DateTimeKind.Local).AddTicks(6348), 2160m, "Course5", 5 },
                    { 6, new DateTime(2022, 7, 21, 3, 59, 25, 201, DateTimeKind.Local).AddTicks(6374), 2670m, "Course6", 1 },
                    { 7, new DateTime(2022, 7, 23, 3, 59, 25, 201, DateTimeKind.Local).AddTicks(6395), 2842m, "Course7", 2 },
                    { 8, new DateTime(2022, 7, 30, 3, 59, 25, 201, DateTimeKind.Local).AddTicks(6417), 3600m, "Course8", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_StudentId",
                table: "Courses",
                column: "StudentId");
            string procInsert = @"CREATE PROC InsertStudent @n VARCHAR(50), @a int, @g INT, @pi VARCHAR(50), @ir BIT
                         AS
                         INSERT INTO Students (StudentName, Age, Gender, Picture, IsRegular)
                         VALUES (@n, @a, @g, @pi, @ir )

                         GO";
            migrationBuilder.Sql(procInsert);
            string procUpdate = @"CREATE PROC UpdateStudent @i INT, @n VARCHAR(50), @a int, @g INT, @pi VARCHAR(50), @ir BIT
                         AS
                         UPDATE Students SET StudentName=@n, Age=@a, Gender=@g, Picture=@pi, IsRegular=@ir
                         WHERE StudentId=@i
                         GO";
            migrationBuilder.Sql(procUpdate);
            string procDelete = @"CREATE PROC DeleteStudent @i INT
                         AS
                         DELETE Students 
                         WHERE StudentId=@i
                         GO";
            migrationBuilder.Sql(procDelete);
            string sqlS = @"CREATE PROC InsertCourse @cn VARCHAR(50), @cf MONEY , @ad DATE, @sid INT
                         AS
                         INSERT INTO Courses (CourseName, CourseFee, AdmissionDate, StudentId)
                         VALUES (@cn, @cf, @ad, @sid )
                         GO";
            migrationBuilder.Sql(sqlS);
            string sqlSU = @"CREATE PROC UpdateCourse @id INT, @cn VARCHAR(50), @cf MONEY , @ad DATE, @sid INT
                         AS
                         UPDATE Courses SET CourseName=@cn, CourseFee=@cf, AdmissionDate=@ad, StudentId=@sid
                         WHERE CourseId = @id
                         GO";
            migrationBuilder.Sql(sqlSU);
            string sqlDU = @"CREATE PROC DeleteCourse @id INT
                     AS
                     DELETE Courses 
                     WHERE CourseId = @id
                     GO";
            migrationBuilder.Sql(sqlDU);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Students");
            migrationBuilder.Sql("DROP PROC InsertStudent");
            migrationBuilder.Sql("DROP PROC UpdateStudent");
            migrationBuilder.Sql("DROP PROC DeleteStudent");
            migrationBuilder.Sql("DROP PROC InsertCourse");
            migrationBuilder.Sql("DROP PROC UpdateCourse");
            migrationBuilder.Sql("DROP PROC DeleteCourse");
        }
    }
}
