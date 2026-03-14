using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrdersService.Api.Infrastructure.Datas.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrderNumber",
                table: "orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.Sql("""
                    CREATE EXTENSION IF NOT EXISTS "pgcrypto";
                """);

            migrationBuilder.Sql("""
                    UPDATE orders
                    SET "OrderNumber" = gen_random_uuid()
                    WHERE "OrderNumber" IS NULL;
                """);

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderNumber",
                table: "orders",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_OrderNumber",
                table: "orders",
                column: "OrderNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_orders_OrderNumber",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "orders");
        }
    }
}
