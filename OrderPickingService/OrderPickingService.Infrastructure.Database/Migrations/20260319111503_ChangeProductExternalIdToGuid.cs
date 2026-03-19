using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderPickingService.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class ChangeProductExternalIdToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Удалить старую колонку
            migrationBuilder.DropColumn(
                name: "product_external_id",
                table: "order_items");
    
            // Создать новую с типом uuid
            migrationBuilder.AddColumn<Guid>(
                name: "product_external_id",
                table: "order_items",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Удалить колонку uuid
            migrationBuilder.DropColumn(
                name: "product_external_id",
                table: "order_items");
    
            // Воссоздать как bigint
            migrationBuilder.AddColumn<long>(
                name: "product_external_id",
                table: "order_items",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}