using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderPickingService.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "user_id",
                table: "orders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "orders",
                keyColumn: "id",
                keyValue: 1L,
                column: "user_id",
                value: "test-user-id-123");

            migrationBuilder.UpdateData(
                table: "orders",
                keyColumn: "id",
                keyValue: 2L,
                column: "user_id",
                value: "test-user-id-123");

            migrationBuilder.UpdateData(
                table: "orders",
                keyColumn: "id",
                keyValue: 3L,
                column: "user_id",
                value: "test-user-id-123");

            migrationBuilder.UpdateData(
                table: "orders",
                keyColumn: "id",
                keyValue: 4L,
                column: "user_id",
                value: "test-user-id-123");

            migrationBuilder.UpdateData(
                table: "orders",
                keyColumn: "id",
                keyValue: 5L,
                column: "user_id",
                value: "test-user-id-123");

            migrationBuilder.UpdateData(
                table: "orders",
                keyColumn: "id",
                keyValue: 6L,
                column: "user_id",
                value: "test-user-id-123");

            migrationBuilder.UpdateData(
                table: "orders",
                keyColumn: "id",
                keyValue: 7L,
                column: "user_id",
                value: "test-user-id-123");

            migrationBuilder.UpdateData(
                table: "orders",
                keyColumn: "id",
                keyValue: 8L,
                column: "user_id",
                value: "test-user-id-123");

            migrationBuilder.UpdateData(
                table: "orders",
                keyColumn: "id",
                keyValue: 9L,
                column: "user_id",
                value: "test-user-id-123");

            migrationBuilder.UpdateData(
                table: "orders",
                keyColumn: "id",
                keyValue: 10L,
                column: "user_id",
                value: "test-user-id-123");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user_id",
                table: "orders");
        }
    }
}
