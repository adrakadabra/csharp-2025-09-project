using System;
using Microsoft.EntityFrameworkCore.Migrations;
using OrderPickingService.Domain.Enums;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OrderPickingService.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "orders",
                columns: new[] { "id", "created_at", "created_by", "deleted_at", "deleted_by", "external_id", "order_status", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { 1L, new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, new Guid("11111111-1111-1111-1111-111111111111"), OrderStatus.Available, null, null },
                    { 2L, new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, new Guid("22222222-2222-2222-2222-222222222222"), OrderStatus.Available, null, null },
                    { 3L, new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, new Guid("33333333-3333-3333-3333-333333333333"), OrderStatus.Available, null, null },
                    { 4L, new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, new Guid("44444444-4444-4444-4444-444444444444"), OrderStatus.Available, null, null },
                    { 5L, new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, new Guid("55555555-5555-5555-5555-555555555555"), OrderStatus.Available, null, null },
                    { 6L, new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, new Guid("66666666-6666-6666-6666-666666666666"), OrderStatus.Available, null, null },
                    { 7L, new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, new Guid("77777777-7777-7777-7777-777777777777"), OrderStatus.Available, null, null },
                    { 8L, new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, new Guid("88888888-8888-8888-8888-888888888888"), OrderStatus.Available, null, null },
                    { 9L, new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, new Guid("99999999-9999-9999-9999-999999999999"), OrderStatus.Available, null, null },
                    { 10L, new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), OrderStatus.Available, null, null }
                });

            migrationBuilder.InsertData(
                table: "pickers",
                columns: new[] { "id", "created_at", "created_by", "deleted_at", "deleted_by", "first_name", "last_name", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { 1L, new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, "Иван", "Петров", null, null },
                    { 2L, new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, "Мария", "Сидорова", null, null },
                    { 3L, new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, "Алексей", "Иванов", null, null },
                    { 4L, new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, "Елена", "Козлова", null, null },
                    { 5L, new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, "Дмитрий", "Смирнов", null, null }
                });

            migrationBuilder.InsertData(
                table: "order_items",
                columns: new[] { "id", "category", "created_at", "created_by", "deleted_at", "deleted_by", "order_id", "price", "product_external_id", "product_name", "product_sku", "quantity", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { 1L, "Молочные продукты", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, 1L, 119.00m, new Guid("40000000-0000-0000-0000-000000000003"), "Молоко", "DAIRY-0001", 2L, null, null },
                    { 2L, "Хлеб и выпечка", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, 1L, 54.00m, new Guid("40000000-0000-0000-0000-000000000006"), "Хлеб белый", "BAKERY-0001", 1L, null, null },
                    { 3L, "Овощи", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, 2L, 79.00m, new Guid("40000000-0000-0000-0000-000000000010"), "Картофель", "VEGET-0001", 3L, null, null },
                    { 4L, "Овощи", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, 2L, 159.00m, new Guid("40000000-0000-0000-0000-000000000011"), "Огурцы", "VEGET-0002", 1L, null, null },
                    { 5L, "Фрукты", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, 2L, 149.00m, new Guid("40000000-0000-0000-0000-000000000008"), "Яблоки", "FRUITS-0001", 2L, null, null },
                    { 6L, "Напитки", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, 3L, 69.00m, new Guid("40000000-0000-0000-0000-000000000022"), "Минеральная вода", "DRINKS-0001", 6L, null, null },
                    { 7L, "Напитки", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, 3L, 149.00m, new Guid("40000000-0000-0000-0000-000000000023"), "Апельсиновый сок", "DRINKS-0002", 2L, null, null },
                    { 8L, "Бытовая химия", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, 4L, 599.00m, new Guid("40000000-0000-0000-0000-000000000012"), "Гель для стирки", "HOME-0001", 1L, null, null },
                    { 9L, "Бытовая химия", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, 4L, 249.00m, new Guid("40000000-0000-0000-0000-000000000013"), "Средство для посуды", "HOME-0002", 2L, null, null },
                    { 10L, "Товары для дома", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, 4L, 129.00m, new Guid("40000000-0000-0000-0000-000000000014"), "Бумажные полотенца", "HOME-0003", 3L, null, null },
                    { 11L, "Мясо и птица", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, 5L, 329.00m, new Guid("40000000-0000-0000-0000-000000000020"), "Куриное филе", "MEAT-0001", 2L, null, null },
                    { 12L, "Мясо и птица", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, 5L, 289.00m, new Guid("40000000-0000-0000-0000-000000000021"), "Фарш говяжий", "MEAT-0002", 1L, null, null },
                    { 13L, "Молочные продукты", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, 6L, 219.00m, new Guid("40000000-0000-0000-0000-000000000004"), "Сыр", "DAIRY-0002", 3L, null, null },
                    { 14L, "Молочные продукты", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, 6L, 89.00m, new Guid("40000000-0000-0000-0000-000000000005"), "Йогурт", "DAIRY-0003", 5L, null, null },
                    { 15L, "Автомобильные товары", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, 7L, 1250.00m, new Guid("40000000-0000-0000-0000-000000000001"), "Машинное масло", "AUTO-0001", 1L, null, null },
                    { 16L, "Автомобильные товары", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, 7L, 349.00m, new Guid("40000000-0000-0000-0000-000000000002"), "Омыватель стекла", "AUTO-0002", 2L, null, null },
                    { 17L, "Электроника", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, 8L, 2999.00m, new Guid("40000000-0000-0000-0000-000000000017"), "Наушники беспроводные", "ELECTRO-0001", 1L, null, null },
                    { 18L, "Электроника", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, 8L, 299.00m, new Guid("40000000-0000-0000-0000-000000000018"), "Кабель USB-C", "ELECTRO-0002", 3L, null, null },
                    { 19L, "Парфюмерия", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, 9L, 3499.00m, new Guid("40000000-0000-0000-0000-000000000015"), "Духи", "BEAUTY-0001", 1L, null, null },
                    { 20L, "Уход за волосами", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, 9L, 389.00m, new Guid("40000000-0000-0000-0000-000000000016"), "Шампунь", "BEAUTY-0002", 2L, null, null },
                    { 21L, "Шины", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, 10L, 6590.00m, new Guid("40000000-0000-0000-0000-000000000019"), "Шины летние", "TIRES-0001", 4L, null, null },
                    { 22L, "Детские товары", new DateTime(2026, 3, 19, 0, 0, 0, 0, DateTimeKind.Utc), "seed", null, null, 10L, 1399.00m, new Guid("40000000-0000-0000-0000-000000000024"), "Подгузники", "BABY-0001", 2L, null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 16L);

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 17L);

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 18L);

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 19L);

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 21L);

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: 22L);

            migrationBuilder.DeleteData(
                table: "pickers",
                keyColumn: "id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "pickers",
                keyColumn: "id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "pickers",
                keyColumn: "id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "pickers",
                keyColumn: "id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "pickers",
                keyColumn: "id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: 10L);
        }
    }
}
