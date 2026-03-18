using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderPickingService.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOrderExternalIdToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Удалить старую колонку
            migrationBuilder.DropColumn(
                name: "external_id",
                table: "orders");
    
            // Создать новую с типом uuid
            migrationBuilder.AddColumn<Guid>(
                name: "external_id",
                table: "orders",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Удалить колонку uuid
            migrationBuilder.DropColumn(
                name: "external_id",
                table: "orders");
    
            // Воссоздать как bigint
            migrationBuilder.AddColumn<long>(
                name: "external_id",
                table: "orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
