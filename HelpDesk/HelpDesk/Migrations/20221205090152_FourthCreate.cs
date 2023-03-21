using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk.Migrations
{
    public partial class FourthCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployeeRequestEmployeeId",
                table: "Requests",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmployeeHandledRequests",
                columns: table => new
                {
                    EmployeeId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeHandledRequests", x => x.EmployeeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_EmployeeRequestEmployeeId",
                table: "Requests",
                column: "EmployeeRequestEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_EmployeeHandledRequests_EmployeeRequestEmployeeId",
                table: "Requests",
                column: "EmployeeRequestEmployeeId",
                principalTable: "EmployeeHandledRequests",
                principalColumn: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_EmployeeHandledRequests_EmployeeRequestEmployeeId",
                table: "Requests");

            migrationBuilder.DropTable(
                name: "EmployeeHandledRequests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_EmployeeRequestEmployeeId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "EmployeeRequestEmployeeId",
                table: "Requests");
        }
    }
}
