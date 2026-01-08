using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderPickingService.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "pickers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "pickers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "pickers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                table: "pickers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "pickers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "pickers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "pickers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "pickers");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "pickers");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "pickers");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "pickers");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "pickers");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "pickers");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "pickers");
        }
    }
}
