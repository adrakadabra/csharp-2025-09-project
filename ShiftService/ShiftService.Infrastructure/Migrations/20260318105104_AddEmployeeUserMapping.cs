using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeUserMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Employees",
                newName: "Id");

            migrationBuilder.AlterColumn<bool>(
                name: "AccessAllowed",
                table: "Employees",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Employees",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Employees",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EmployeeUserMappings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    KeycloakUserId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    KeycloakUsername = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeUserMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeUserMappings_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_FullName",
                table: "Employees",
                column: "FullName");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_QrCode",
                table: "Employees",
                column: "QrCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeUserMappings_EmployeeId",
                table: "EmployeeUserMappings",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeUserMappings_KeycloakUserId",
                table: "EmployeeUserMappings",
                column: "KeycloakUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeUserMappings_KeycloakUsername",
                table: "EmployeeUserMappings",
                column: "KeycloakUsername",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeUserMappings");

            migrationBuilder.DropIndex(
                name: "IX_Employees_FullName",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_QrCode",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Employees",
                newName: "EmployeeId");

            migrationBuilder.AlterColumn<bool>(
                name: "AccessAllowed",
                table: "Employees",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);
        }
    }
}
