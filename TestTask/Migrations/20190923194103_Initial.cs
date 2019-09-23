using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestTask.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: false),
                    SecondName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "DATE", nullable: false),
                    Gender = table.Column<string>(nullable: false),
                    SNILS = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vaccines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vaccines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vaccinations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VaccineName = table.Column<string>(nullable: false),
                    Consent = table.Column<bool>(nullable: false),
                    Date = table.Column<DateTime>(type: "DATE", nullable: false),
                    PatientId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vaccinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vaccinations_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "DateOfBirth", "FirstName", "Gender", "LastName", "SNILS", "SecondName" },
                values: new object[,]
                {
                    { 1, new DateTime(1980, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Иван", "м", "Васильев", "112-233-445 95", null },
                    { 2, new DateTime(1990, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Василий", "м", "Иванов", "112-233-446 96", null },
                    { 3, new DateTime(1995, 7, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Алексей", "м", "Смирнов", "112-233-436 94", null },
                    { 4, new DateTime(1965, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Петр", "м", "Иванов", "111-233-446 89", null },
                    { 5, new DateTime(1985, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Валентина", "ж", "Иванова", "111-133-446 83", null }
                });

            migrationBuilder.InsertData(
                table: "Vaccines",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Эджерикс" },
                    { 2, "Вианвак" },
                    { 3, "АКДС" },
                    { 4, "БЦЖ" }
                });

            migrationBuilder.InsertData(
                table: "Vaccinations",
                columns: new[] { "Id", "Consent", "Date", "PatientId", "VaccineName" },
                values: new object[,]
                {
                    { 1, false, new DateTime(2019, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Эджерикс" },
                    { 2, false, new DateTime(2019, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Вианвак" },
                    { 3, true, new DateTime(2019, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "АКДС" },
                    { 4, true, new DateTime(2019, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "БЦЖ" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vaccinations_PatientId",
                table: "Vaccinations",
                column: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vaccinations");

            migrationBuilder.DropTable(
                name: "Vaccines");

            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}
