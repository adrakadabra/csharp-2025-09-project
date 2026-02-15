using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderPickingService.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddPickerColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "pickers",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "last_name",
                table: "pickers",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "first_name",
                table: "pickers");

            migrationBuilder.DropColumn(
                name: "last_name",
                table: "pickers");
        }
    }
}
